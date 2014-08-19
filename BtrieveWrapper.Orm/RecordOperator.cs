using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class RecordOperator<TRecord> : IDisposable where TRecord : Record<TRecord>
    {
        static byte[] _keyBuffer = new byte[255];

        PositionBlock _positionBlock = null;
        Func<byte[], object> _recordConstructor;
        int _reusableCapacity;
        Queue<TRecord> _reusableRecords;
        Operator _nativeOperator;

        public RecordOperator(string dllPath = null, Path path = null, string ownerName = null, OpenMode? openMode = null, int reusableCapacity = 1000, byte[] temporaryBuffer = null)
            : this(
                new Operator(
                    dllPath ?? Resource.GetRecordInfo(typeof(TRecord)).DllPath,
                    false),
                path,
                ownerName,
                openMode,
                reusableCapacity,
                temporaryBuffer) { }

        public RecordOperator(string applicationId, ushort threadId, string dllPath = null, Path path = null, string ownerName = null, OpenMode? openMode = null, int reusableCapacity = 1000, byte[] temporaryBuffer = null)
            : this(
                new Operator(
                    dllPath ?? Resource.GetRecordInfo(typeof(TRecord)).DllPath,
                    true),
                path,
                ownerName,
                openMode,
                reusableCapacity,
                temporaryBuffer) {
            _nativeOperator.ClientId.ApplicationId = applicationId;
            _nativeOperator.ClientId.ThreadId = threadId;
        }

        public RecordOperator(Operator nativeOperator, Path path, string ownerName, OpenMode? openMode, int reusableCapacity, byte[] temporaryBuffer = null) {
            var recordType = typeof(TRecord);
            var recordConstructor = recordType.GetConstructor(new Type[] { typeof(byte[]) });
            if (recordConstructor == null) {
                throw new InvalidDefinitionException();
            }
            _recordConstructor = Resource.GetRecordConstructor(typeof(TRecord));
            this.RecordInfo = Resource.GetRecordInfo(recordType);
            if (temporaryBuffer != null && temporaryBuffer.Length < this.RecordInfo.DataBufferCapacity) {
                throw new ArgumentException();
            }
            this.PrimaryKeyNumber = this.RecordInfo.PrimaryKeyNumber;
            _nativeOperator = nativeOperator;
            this.IsClosable = true;
            this.HasVariableFields = this.RecordInfo.VariableOption != RecordVariableOption.NotVariable && this.RecordInfo.VariableRangeFields.Count() != 0;
            this.Path = Path.Merge(path, this.RecordInfo);
            this.OwnerName = ownerName;
            this.OpenMode = openMode;
            this.TemporaryBuffer = temporaryBuffer ?? new byte[ushort.MaxValue];
            _reusableCapacity = reusableCapacity;
            _reusableRecords = new Queue<TRecord>(_reusableCapacity);
        }

        public sbyte PrimaryKeyNumber { get; private set; }
        public StatData Stat { get; private set; }
        public bool IsClosable { get; internal set; }
        public bool HasVariableFields { get; private set; }

        internal RecordInfo RecordInfo { get; private set; }
        internal byte[] TemporaryBuffer { get; private set; }

        public Path Path { get; set; }
        public string OwnerName { get; set; }
        public OpenMode? OpenMode { get; set; }

        public bool IsOpened { get { return _positionBlock != null; } }


        static IEnumerable<ushort> GetUpdatedPositions(FieldInfo field) {
            for (var i = 0; i < field.Length; i++) {
                yield return (ushort)(field.Position + i);
            }
        }

        public TRecord DeepCopy(TRecord record) {
            var dataBuffer = new byte[record.DataBuffer.Length];
            Array.Copy(record.DataBuffer, 0, dataBuffer, 0, record.DataBuffer.Length);
            return (TRecord)_recordConstructor(dataBuffer);
        }

        public TRecord CreateRecord(byte[] dataBuffer, bool byReference = false) {
            if (dataBuffer == null) {
                throw new ArgumentNullException();
            }
            if (dataBuffer.Length != this.RecordInfo.DataBufferCapacity) {
                throw new ArgumentException();
            }
            if (_reusableRecords.Count > 0) {
                var result = _reusableRecords.Dequeue();
                result.Initialize(dataBuffer, byReference);
                return result;
            } else {
                return (TRecord)_recordConstructor(dataBuffer);
            }
        }

        internal TRecord CreateRecord() {
            return _reusableRecords.Count > 0
                ? _reusableRecords.Dequeue()
                : (TRecord)_recordConstructor(new byte[this.RecordInfo.DataBufferCapacity]);
        }

        public void Recycle(TRecord record) {
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (record.RecordState != RecordState.Detached) {
                throw new ArgumentException();
            }
            if (_reusableRecords.Count < _reusableCapacity) {
                record.HasPhysicalPosition = false;
                record.PhysicalPosition = 0;
                _reusableRecords.Enqueue(record);
            }
        }

        internal void RecycleInternalRecord(TRecord record) {
#if DEBUG
            if (record.RecordState != RecordState.Detached) {
                throw new ArgumentException();
            }
#endif
            if (_reusableRecords.Count < _reusableCapacity) {
                _reusableRecords.Enqueue(record);
            }
        }

        public void Create(Path path = null, bool overwrite = false) {
            var record = this.RecordInfo;
            var fileSpec = record.GetCreateFileSpec();
            var keySpecs = record.Keys.SelectMany(k => k.Segments.Select(s => s.GetCreateKeySpec()));
            var createData = new CreateData(fileSpec, keySpecs);
            var filePath = Path.Merge(path, this.Path).GetFilePath();
            _nativeOperator.Create(filePath, createData, overwrite);
            if(this.RecordInfo.OwnerName!=null){
                var positionBlock=_nativeOperator.Open(filePath);
                try {
                    _nativeOperator.SetOwner(positionBlock, this.RecordInfo.OwnerName, this.RecordInfo.OwnerNameOption);
                } finally {
                    _nativeOperator.Close(positionBlock);
                }
            }
        }

        public void Open(Path path = null) {
            var filePath = Path.Merge(path, this.Path).GetFilePath();
            _positionBlock = _nativeOperator.Open(filePath, this.OwnerName ?? this.RecordInfo.OwnerName, this.OpenMode ?? this.RecordInfo.OpenMode);
            if (this.Stat == null) {
                this.Stat = _nativeOperator.Stat(_positionBlock, this.TemporaryBuffer);
                if (this.RecordInfo.FixedLength != this.Stat.FileSpec.RecordLength ||
                    this.RecordInfo.VariableOption != this.Stat.FileSpec.VariableOption) {
                    throw new InvalidDefinitionException();
                }
            }
        }

        public void Close() {
            if (IsClosable) {
                if (_positionBlock != null) {
                    _nativeOperator.Close(_positionBlock);
                    _positionBlock = null;
                }
            } else {
                throw new InvalidOperationException();
            }
        }

        public void Insert(TRecord record) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (record == null) {
                throw new ArgumentNullException();
            }
            _nativeOperator.Insert(_positionBlock, record.DataBuffer, (ushort)record.DataBuffer.Length, -1, 0);
        }
        
        public void InsertExtended(IEnumerable<TRecord> records, Action<IEnumerable<TRecord>> onInserted = null) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (records == null) {
                throw new ArgumentNullException();
            }
            ushort position = 2;
            var recordList=new List<TRecord>();
            ushort capacity = (ushort)(Config.MaxBufferLength - 412);
            foreach (var record in records) {
                if (position + this.RecordInfo.DataBufferCapacity + 2 > capacity) {
                    Array.Copy(BitConverter.GetBytes(recordList.Count), this.TemporaryBuffer, 2);
                    OperationException exception = null;
                    try {
                        _nativeOperator.InsertExtended(_positionBlock, this.TemporaryBuffer, capacity);
                    } catch (OperationException e) {
                        if (e.StatusCode == 5) {
                            exception = e;
                        } else {
                            throw;
                        }
                    }
                    var count = BitConverter.ToUInt16(this.TemporaryBuffer, 0);
                    position = 2;
                    for (var i = 0; i < count; i++) {
                        recordList[i].PhysicalPosition = BitConverter.ToUInt32(this.TemporaryBuffer, position);
                        recordList[i].HasPhysicalPosition = true;
                        position += 4;
                    }
                    if (onInserted != null) {
                        if (exception == null) {
                            onInserted(recordList.Select(r => r));
                        } else {
                            onInserted(recordList.Take(count));
                        }
                    }
                    if (exception != null) {
                        throw exception;
                    }
                    position = 2;
                    recordList.Clear();
                }
                ushort recordLength = this.RecordInfo.DataBufferCapacity;
                Array.Copy(BitConverter.GetBytes(recordLength), 0, this.TemporaryBuffer, position, 2);
                Array.Copy(record.DataBuffer, 0, this.TemporaryBuffer, position + 2, this.RecordInfo.DataBufferCapacity);
                position += (ushort)(recordLength + 2);
                recordList.Add(record);
            }
            if (recordList.Count != 0) {
                Array.Copy(BitConverter.GetBytes(recordList.Count), this.TemporaryBuffer, 2);
                _nativeOperator.InsertExtended(_positionBlock, this.TemporaryBuffer, capacity);
                if (onInserted != null) {
                    onInserted(recordList.Select(r => r));
                }
            }
        }

        public void Update(TRecord record) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (this.HasVariableFields) {
                var position = 0;
                Array.Copy(BitConverter.GetBytes(0x80000000), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                Array.Copy(BitConverter.GetBytes(1), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                Array.Copy(BitConverter.GetBytes(0), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                Array.Copy(BitConverter.GetBytes((int)this.RecordInfo.DataBufferCapacity), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                if (Resource.Is64bit) {
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                } else {
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                }
                Array.Copy(record.DataBuffer, 0, this.TemporaryBuffer, position, this.RecordInfo.DataBufferCapacity);
                var dataBufferLength = (ushort)(position + this.RecordInfo.DataBufferCapacity);
                _nativeOperator.UpdateChunk(_positionBlock, this.TemporaryBuffer, ref dataBufferLength);
            } else {
                _nativeOperator.Update(_positionBlock, record.DataBuffer);
            }
        }

        public void UpdateChunk(params UpdateChunkParameter[] parameters) {
            this.UpdateChunk(parameters);
        }

        public void UpdateChunk(IEnumerable<UpdateChunkParameter> parameters) {
            if (parameters == null) {
                throw new ArgumentNullException();
            }
            var parameterCount = parameters.Count();
            if (parameterCount == 0) {
                throw new ArgumentException();
            }

            var position = 0;
            var dataPosition = 8 + (Resource.Is64bit ? 16 : 12) * parameterCount;
            Array.Copy(BitConverter.GetBytes(0x80000000), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(parameterCount), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            foreach (var parameter in parameters) {
                Array.Copy(BitConverter.GetBytes(parameter.Range.Position), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                Array.Copy(BitConverter.GetBytes(parameter.Range.Length), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                if (Resource.Is64bit) {
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                } else {
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                }
                Array.Copy(parameter.Data, parameter.DataPosition, this.TemporaryBuffer, dataPosition, parameter.Range.Length);
                dataPosition += parameter.Range.Length;
            }
            var dataBufferLength = (ushort)dataPosition;
            _nativeOperator.UpdateChunk(_positionBlock, this.TemporaryBuffer, ref dataBufferLength);
        }

        public void UpdateChunk(int cutOffPosition) {
            if (cutOffPosition < this.RecordInfo.VariableFieldCapacity) {
                throw new ArgumentException();
            }
            Array.Copy(BitConverter.GetBytes(0x80000004), 0, this.TemporaryBuffer, 0, 4);
            Array.Copy(BitConverter.GetBytes(cutOffPosition), 0, this.TemporaryBuffer, 4, 4);
            var dataBufferLength = (ushort)8;
            _nativeOperator.UpdateChunk(_positionBlock, this.TemporaryBuffer, ref dataBufferLength);
        }

        public void Delete() {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            _nativeOperator.Delete(_positionBlock);
        }

        public TRecord StepFirst(LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            var record = this.CreateRecord();
            _nativeOperator.StepFirst(_positionBlock, record.DataBuffer, lockBias);
            return record;
        }

        public TRecord StepLast(LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            var record = this.CreateRecord();
            _nativeOperator.StepLast(_positionBlock, record.DataBuffer, lockBias);
            return record;
        }

        public TRecord StepNext(LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            var record = this.CreateRecord();
            _nativeOperator.StepNext(_positionBlock, record.DataBuffer, lockBias);
            return record;
        }

        public TRecord StepPrevious(LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            var record = this.CreateRecord();
            _nativeOperator.StepPrevious(_positionBlock, record.DataBuffer, lockBias);
            return record;
        }

        public TRecord GetFirst(KeyInfo key, LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key == null) {
                throw new ArgumentNullException();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetFirst(_positionBlock, record.DataBuffer, _keyBuffer, key.KeyNumber, lockBias);
            return record;
        }

        public TRecord GetLast(KeyInfo key, LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key == null) {
                throw new ArgumentNullException();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetLast(_positionBlock, record.DataBuffer, _keyBuffer, key.KeyNumber, lockBias);
            return record;
        }

        public TRecord GetNext(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetNext(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public TRecord GetPrevious(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetPrevious(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public TRecord GetEqual(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (keyValue.ComplementCount != 0) {
                throw new ArgumentException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetEqual(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public TRecord GetGreater(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetGreater(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public TRecord GetGreaterThanOrEqual(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetGreaterThanOrEqual(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public TRecord GetLessThan(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetLessThan(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public TRecord GetLessThanOrEqual(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var record = this.CreateRecord();
            _nativeOperator.GetLessThanOrEqual(_positionBlock, record.DataBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return record;
        }

        public KeyValue GetFirstKey(KeyInfo key, LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key == null) {
                throw new ArgumentNullException();
            }
            var keyBuffer = new byte[key.Length];
            _nativeOperator.GetFirstKey(_positionBlock, keyBuffer, key.KeyNumber, lockBias);
            return new KeyValue(key, keyBuffer);
        }

        public KeyValue GetLastKey(KeyInfo key, LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key == null) {
                throw new ArgumentNullException();
            }
            var keyBuffer = new byte[key.Length];
            _nativeOperator.GetLastKey(_positionBlock, keyBuffer, key.KeyNumber, lockBias);
            return new KeyValue(key, keyBuffer);
        }

        public KeyValue GetNextKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetNextKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }

        public KeyValue GetPreviousKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetPreviousKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }
        public KeyValue GetEqualKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (keyValue.ComplementCount != 0) {
                throw new ArgumentException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetEqualKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }

        public KeyValue GetGreaterKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetGreaterKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }
        public KeyValue GetGreaterThanOrEqualKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetGreaterThanOrEqualKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }

        public KeyValue GetLessThanKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetLessThanKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }

        public KeyValue GetLessThanOrEqualKey(KeyValue keyValue, LockBias lockBias = LockBias.None, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (!overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            _nativeOperator.GetLessThanOrEqualKey(_positionBlock, keyValue.KeyBuffer, keyValue.Key.KeyNumber, lockBias);
            keyValue.ComplementCount = 0;
            return keyValue;
        }

        public IEnumerable<TRecord> StepNextExtended(FilterAnd filter, bool skipCurrentPosition = true, int limit = 0, LockBias lockBias = LockBias.None, ushort rejectCount = 0) {
            return this.OperateExtended(Operation.StepNextExtended, null, filter, skipCurrentPosition, limit, lockBias, rejectCount);
        }

        public IEnumerable<TRecord> StepPreviousExtended(FilterAnd filter, bool skipCurrentPosition = true, int limit = 0, LockBias lockBias = LockBias.None, ushort rejectCount = 0) {
            return this.OperateExtended(Operation.StepPreviousExtended, null, filter, skipCurrentPosition, limit, lockBias, rejectCount);
        }

        public IEnumerable<TRecord> GetNextExtended(KeyValue keyValue, FilterAnd filter, bool skipCurrentPosition = true, int limit = 0, LockBias lockBias = LockBias.None, ushort rejectCount = 0, bool overwrite = false) {
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            return this.OperateExtended(Operation.GetNextExtended, keyValue, filter, skipCurrentPosition, limit, lockBias, rejectCount, overwrite);
        }

        public IEnumerable<TRecord> GetPreviousExtended(KeyValue keyValue, FilterAnd filter, bool skipCurrentPosition = true, int limit = 0, LockBias lockBias = LockBias.None, ushort rejectCount = 0, bool overwrite = false) {
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            return this.OperateExtended(Operation.GetPreviousExtended, keyValue, filter, skipCurrentPosition, limit, lockBias, rejectCount, overwrite);
        }

        IEnumerable<TRecord> OperateExtended(Operation operation, KeyValue keyValue, FilterAnd filter, bool skipCurrentPosition, int limit, LockBias lockBias, ushort rejectCount, bool overwrite = false) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (filter != null && !filter.Check(this.RecordInfo)) {
                throw new ArgumentException();
            }
            if (keyValue != null && !overwrite) {
                keyValue = keyValue.DeepCopy();
            }
            var useLimit = limit > 0;
            rejectCount = rejectCount == 0 ? this.RecordInfo.RejectCount : rejectCount;
            var isFirst = true;
            for (; ; ) {
                var dataBufferLength = (ushort)(filter == null ? 16 : filter.Length + 16);
                Array.Copy(BitConverter.GetBytes(dataBufferLength), 0, this.TemporaryBuffer, 0, 2);
                Array.Copy(Encoding.ASCII.GetBytes(!isFirst || skipCurrentPosition ? "EG" : "UC"), 0, this.TemporaryBuffer, 2, 2);
                isFirst = false;
                skipCurrentPosition = true;
                Array.Copy(BitConverter.GetBytes(rejectCount), 0, this.TemporaryBuffer, 4, 2);
                Array.Copy(BitConverter.GetBytes(filter == null ? 0 : filter.Count), 0, this.TemporaryBuffer, 6, 2);
                if (filter != null) {
                    filter.SetDataBuffer(this.TemporaryBuffer);
                }
                var position = (ushort)(filter == null ? 8 : filter.Length + 8);
                var count = (ushort)((Config.MaxBufferLength) / (this.RecordInfo.DataBufferCapacity + 6));
                if (useLimit && count > limit) {
                    count = (ushort)limit;
                }
                if (count == 0) {
                    throw new InvalidOperationException();
                }
                Array.Copy(BitConverter.GetBytes(count), 0, this.TemporaryBuffer, position, 2);
                position += 2;
                Array.Copy(BitConverter.GetBytes((ushort)(1)), 0, this.TemporaryBuffer, position, 2);
                position += 2;
                Array.Copy(BitConverter.GetBytes(this.RecordInfo.DataBufferCapacity), 0, this.TemporaryBuffer, position, 2);
                position += 2;
                Array.Copy(BitConverter.GetBytes((ushort)(0)), 0, this.TemporaryBuffer, position, 2);
                var isBreak = false;
                try {
                    if (keyValue == null) {
                        _nativeOperator.Operate(operation, _positionBlock, this.TemporaryBuffer, null, (sbyte)0, (ushort)lockBias);
                    } else {
                        _nativeOperator.Operate(operation, _positionBlock, this.TemporaryBuffer, keyValue.KeyBuffer, keyValue.Key.KeyNumber, (ushort)lockBias);
                    }
                } catch (OperationException e) {
                    if (e.StatusCode == 9 || e.StatusCode == 64) {
                        isBreak = true;
                    } else if (e.StatusCode != 60) {
                        throw;
                    }
                }
                if (keyValue != null) {
                    keyValue.ComplementCount = 0;
                }

                count = BitConverter.ToUInt16(this.TemporaryBuffer, 0);
                position = 2;
                ushort length;
                TRecord record;
                for (var i = 0; i < count; i++) {
                    length = BitConverter.ToUInt16(this.TemporaryBuffer, position);
                    record = this.CreateRecord();
                    Array.Copy(this.TemporaryBuffer, position + 6, record.DataBuffer, 0, this.RecordInfo.DataBufferCapacity);
                    record.HasPhysicalPosition = true;
                    record.PhysicalPosition = BitConverter.ToUInt32(this.TemporaryBuffer, position + 2);
                    yield return record;
                    position += (ushort)(length + 6);
                }
                if (isBreak) {
                    yield break;
                }
                if (useLimit) {
                    limit -= count;
                    if (limit <= 0) {
                        yield break;
                    }
                }
            }
        }

        public uint GetPosition() {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            _nativeOperator.GetPosition(_positionBlock, this.TemporaryBuffer);
            return BitConverter.ToUInt32(this.TemporaryBuffer, 0);
        }

        public TRecord GetRecord(uint position, KeyInfo key = null, LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key != null && key.Record != this.RecordInfo) {
                throw new ArgumentException();
            }
            var keyNumber = key == null ? (sbyte)-1 : key.KeyNumber;
            var record = this.CreateRecord();
            if (record.DataBuffer.Length < 4) {
                Array.Copy(BitConverter.GetBytes(position), this.TemporaryBuffer, 4);
                var dataBufferLength = (ushort)this.TemporaryBuffer.Length;
                _nativeOperator.GetDirect(_positionBlock, this.TemporaryBuffer, ref dataBufferLength, _keyBuffer, keyNumber, lockBias);
                Array.Copy(this.TemporaryBuffer, record.DataBuffer, record.DataBuffer.Length);
            } else {
                Array.Copy(BitConverter.GetBytes(position), record.DataBuffer, 4);
                _nativeOperator.GetDirect(_positionBlock, record.DataBuffer, null, -1, lockBias);
            }
            return record;
        }

        public List<byte[]> GetChunk(uint physicalPosition, IEnumerable<VariableRange> chunkParameters) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (chunkParameters == null) {
                throw new ArgumentNullException();
            }
            int recieveLength = 0, count = 0;
            foreach (var range in chunkParameters) {
                recieveLength += range.Length;
                count++;
            }
            var sendLength = 12 + (Resource.Is64bit ? 16 : 12) * count;
            if (count == 0 || recieveLength > 0xfc00) {
                throw new ArgumentException();
            }
            var dataBufferLength = sendLength > recieveLength ? (ushort)sendLength : (ushort)recieveLength;
            var position = 0;
            Array.Copy(BitConverter.GetBytes(physicalPosition), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(0x80000000), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(count), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            foreach (var range in chunkParameters) {
                Array.Copy(BitConverter.GetBytes(range.Position), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                Array.Copy(BitConverter.GetBytes(range.Length), 0, this.TemporaryBuffer, position, 4);
                position += 4;
                if (Resource.Is64bit) {
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                } else {
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                    this.TemporaryBuffer[position++] = 0;
                }
            }
            _nativeOperator.UpdateChunk(_positionBlock, this.TemporaryBuffer, ref dataBufferLength, null, -2);

            var result = new List<byte[]>();
            position = 0;
            foreach (var range in chunkParameters) {
                var buffer = new byte[range.Length];
                Array.Copy(this.TemporaryBuffer, position, buffer, 0, range.Length);
                result.Add(buffer);
                position += range.Length;
            }
            return result;
        }

        public byte[] GetChunk(uint physicalPosition, VariableRange chunkParameter) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            int recieveLength = chunkParameter.Length;
            var sendLength = 12 + (Resource.Is64bit ? 16 : 12);
            if (recieveLength > 0xfc00) {
                throw new ArgumentException();
            }
            var dataBufferLength = sendLength > recieveLength ? (ushort)sendLength : (ushort)recieveLength;
            var position = 0;
            Array.Copy(BitConverter.GetBytes(physicalPosition), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(0x80000000), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(1), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(chunkParameter.Position), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            Array.Copy(BitConverter.GetBytes(chunkParameter.Length), 0, this.TemporaryBuffer, position, 4);
            position += 4;
            if (Resource.Is64bit) {
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
            } else {
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
                this.TemporaryBuffer[position++] = 0;
            }
            _nativeOperator.UpdateChunk(_positionBlock, this.TemporaryBuffer, ref dataBufferLength, null, -2);

            var result = new byte[chunkParameter.Length];
            Array.Copy(this.TemporaryBuffer, 0, result, 0, chunkParameter.Length);
            return result;
        }

        public void SetPosition(uint position, KeyInfo key = null, LockBias lockBias = LockBias.None) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key != null && key.Record != this.RecordInfo) {
                throw new ArgumentException();
            }
            var keyNumber = key == null ? (sbyte)-1 : key.KeyNumber;
            Array.Copy(BitConverter.GetBytes(position), this.TemporaryBuffer, 4);
            var dataBufferLength = this.RecordInfo.DataBufferCapacity;
            _nativeOperator.GetDirect(_positionBlock, this.TemporaryBuffer, ref dataBufferLength, _keyBuffer, keyNumber, lockBias);
        }

        public void UnlockSingleRecord(sbyte keyNumber) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            _nativeOperator.UnlockSingleRecord(_positionBlock, keyNumber);
        }

        public void UnlockSingleRecord(KeyInfo key) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            if (key == null) {
                throw new ArgumentNullException();
            }
            _nativeOperator.UnlockSingleRecord(_positionBlock, key.KeyNumber);
        }

        public void UnlockMultiRecord() {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            _nativeOperator.UnlockMultiRecord(_positionBlock, null);
        }

        public void UnlockMultiRecord(uint recordPosition) {
            if (_positionBlock == null) {
                throw new InvalidOperationException();
            }
            _nativeOperator.UnlockMultiRecord(_positionBlock, BitConverter.GetBytes(recordPosition));
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing && _positionBlock != null) {
                _nativeOperator.Close(_positionBlock);
                _positionBlock = null;
            }
        }

        public void Dispose() {
            this.Dispose(true);
        }

    }
}
