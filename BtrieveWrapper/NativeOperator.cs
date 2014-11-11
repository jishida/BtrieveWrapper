using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper;
using BtrieveWrapper.Utilities;

namespace BtrieveWrapper
{
    public class NativeOperator
    {
        static readonly byte[] _zero = new byte[0];

        INativeLibrary _library;

        public NativeOperator(string dllPath = null, IEnumerable<string> dependencyPaths = null) 
            : this(NativeLibrary.GetNativeLibrary(dllPath, dependencyPaths)) { }

        public NativeOperator(INativeLibrary nativeLibrary) {
            _library = nativeLibrary;
            this.OwnerNameEncoding = Encoding.ASCII;
            this.PathEncoding = Encoding.Default;
        }

        public NativeOperator(string applicationId, ushort threadId, string dllPath = null, IEnumerable<string> dependencyPaths = null)
            : this(applicationId, threadId, NativeLibrary.GetNativeLibrary(applicationId, dependencyPaths)) { }

        public NativeOperator(string applicationId, ushort threadId, INativeLibrary nativeLibrary) {
            _library = nativeLibrary;
            this.OwnerNameEncoding = Encoding.Default;
            this.PathEncoding = Encoding.Default;
            this.ClientId = new ClientId(applicationId, threadId);
        }

        public Encoding OwnerNameEncoding { get; set; }
        public Encoding PathEncoding { get; set; }
        public ClientId ClientId { get; private set; }


        public void Operate(
            Operation operation,
            PositionBlock positionBlock,
            byte[] dataBuffer,
            byte[] keyBuffer,
            sbyte keyNumber,
            ushort bias = 0) {

            var dataBufferLength = dataBuffer == null ? (ushort)0 : (ushort)dataBuffer.Length;
            this.Operate(
                operation,
                positionBlock,
                dataBuffer,
                ref dataBufferLength,
                keyBuffer,
                keyNumber,
                bias);
        }

        public void Operate(
            Operation operation,
            PositionBlock positionBlock,
            byte[] dataBuffer,
            ref ushort dataBufferLength,
            byte[] keyBuffer,
            sbyte keyNumber,
            ushort bias = 0) {

            if (dataBuffer == null) {
                dataBuffer = _zero;
            }
            if (keyBuffer == null) {
                keyBuffer = _zero;
            }
            if (keyBuffer.Length > 255) {
                throw new ArgumentException();
            }

            byte[] posBlk = positionBlock == null ? _zero : positionBlock.Value;
            short statusCode;
            if (this.ClientId == null) {
                statusCode = _library.BtrCall((ushort)((ushort)operation + bias), posBlk, dataBuffer, ref dataBufferLength, keyBuffer, (ushort)keyBuffer.Length, keyNumber);
            } else {
                statusCode = _library.BtrCallId((ushort)((ushort)operation + bias), posBlk, dataBuffer, ref dataBufferLength, keyBuffer, (ushort)keyBuffer.Length, keyNumber, this.ClientId.Buffer);
            }
            if (statusCode != 0) {
                throw new OperationException(operation, statusCode);
            }
        }

        public PositionBlock Open(string filePath, string ownerName = null, OpenMode openMode = OpenMode.Normal) {
            if (filePath == null) {
                throw new ArgumentNullException();
            }
            filePath = filePath.TrimEnd('\0') + "\0";
            ownerName = ownerName == null ? "\0" : ownerName.TrimEnd('\0') + "\0";

            var positionBlock = new PositionBlock();
            var dataBuffer = (this.OwnerNameEncoding ?? Encoding.Default).GetBytes(ownerName);
            var keyBuffer = (this.PathEncoding ?? Encoding.Default).GetBytes(filePath);
            var keyNumber = (sbyte)openMode;
            this.Operate(Operation.Open, positionBlock, dataBuffer, keyBuffer, keyNumber);

            return positionBlock;
        }

        public void Close(PositionBlock positionBlock) {
            if (positionBlock == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.Close, positionBlock, null, null, keyNumber);
        }

        public StatData Stat(PositionBlock positionBlock) {
            return this.Stat(positionBlock, new byte[1920]);
        }

        public StatData Stat(PositionBlock positionBlock, byte[] dataBuffer) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyBuffer = new byte[64];
            var keyNumber = (sbyte)-1;
            this.Operate(Operation.Stat, positionBlock, dataBuffer, keyBuffer, keyNumber);

            return new StatData(dataBuffer);
        }

        public void SetOwner(PositionBlock positionBlock, string ownerName, OwnerNameOption option = OwnerNameOption.NoEncryption) {
            if (positionBlock == null || ownerName == null) {
                throw new ArgumentNullException();
            }
            ownerName = ownerName == null ? "\0" : ownerName.TrimEnd('\0') + "\0";

            var dataBuffer = (this.OwnerNameEncoding ?? Encoding.Default).GetBytes(ownerName);
            var keyBuffer = (this.OwnerNameEncoding ?? Encoding.Default).GetBytes(ownerName);
            var keyNumber = (sbyte)option;
            this.Operate(Operation.SetOwner, positionBlock, dataBuffer, keyBuffer, keyNumber);
        }

        public void ClearOwner(PositionBlock positionBlock) {
            if (positionBlock == null) {
                throw new ArgumentNullException();
            }
            var dataBuffer = _zero;
            var keyBuffer = _zero;
            var keyNumber = (sbyte)0;
            this.Operate(Operation.ClearOwner, positionBlock, dataBuffer, keyBuffer, keyNumber);
        }

        public void Create(string filePath, CreateData createData, bool overwrite = false) {
            if (filePath == null || createData == null) {
                throw new ArgumentNullException();
            }
            filePath = filePath.TrimEnd('\0') + "\0";

            var dataBuffer = createData.DataBuffer;
            var keyBuffer = (this.PathEncoding ?? Encoding.Default).GetBytes(filePath);
            var keyNumber = overwrite ? (sbyte)0 : (sbyte)-1;
            this.Operate(Operation.Create, null, dataBuffer, keyBuffer, keyNumber);
        }

        public void StepFirst(PositionBlock positionBlock, byte[] dataBuffer, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.StepFirst(positionBlock, dataBuffer, ref dataBufferLength, lockBias);
        }

        public void StepFirst(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength , LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.StepFirst, positionBlock, dataBuffer, ref dataBufferLength, null, keyNumber, (ushort)lockBias);
        }

        public void StepLast(PositionBlock positionBlock, byte[] dataBuffer, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.StepLast(positionBlock, dataBuffer, ref dataBufferLength, lockBias);
        }

        public void StepLast(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.StepLast, positionBlock, dataBuffer, ref dataBufferLength, null, keyNumber, (ushort)lockBias);
        }

        public void StepNext(PositionBlock positionBlock, byte[] dataBuffer, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.StepNext(positionBlock, dataBuffer, ref dataBufferLength, lockBias);
        }

        public void StepNext(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.StepNext, positionBlock, dataBuffer, ref dataBufferLength, null, keyNumber, (ushort)lockBias);
        }

        public void StepNextExtended(PositionBlock positionBlock, byte[] dataBuffer, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.StepNextExtended(positionBlock, dataBuffer, ref dataBufferLength, lockBias);
        }

        public void StepNextExtended(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.StepNextExtended, positionBlock, dataBuffer, ref dataBufferLength, null, keyNumber, (ushort)lockBias);
        }

        public void StepPrevious(PositionBlock positionBlock, byte[] dataBuffer, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.StepPrevious(positionBlock, dataBuffer, ref dataBufferLength, lockBias);
        }

        public void StepPrevious(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.StepPrevious, positionBlock, dataBuffer, ref dataBufferLength, null, keyNumber, (ushort)lockBias);
        }

        public void StepPreviousExtended(PositionBlock positionBlock, byte[] dataBuffer, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.StepPreviousExtended(positionBlock, dataBuffer, ref dataBufferLength, lockBias);
        }

        public void StepPreviousExtended(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.StepPreviousExtended, positionBlock, dataBuffer, ref dataBufferLength, null, keyNumber, (ushort)lockBias);
        }

        public void GetFirst(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetFirst(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetFirst(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetFirst, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetFirstKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetFirstKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetLast(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetLast(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetLast(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetLast, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetLastKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetLastKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetNext(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetNext(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetNext(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetNext, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetNextKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetNextKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetNextExtended(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetNextExtended(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetNextExtended(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetNextExtended, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetPrevious(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetPrevious(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetPrevious(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetPrevious, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetPreviousKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetPreviousKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetPreviousExtended(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetPreviousExtended(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetPreviousExtended(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetPreviousExtended, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetEqual(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetEqual(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetEqual(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetEqual, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetEqualKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetEqualKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetGreater(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetGreater(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetGreater(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetGreater, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetGreaterKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetGreaterKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetGreaterThanOrEqual(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetGreaterThanOrEqual(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetGreaterThanOrEqual(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetGreaterThanOrEqual, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetGreaterThanOrEqualKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetGreaterThanOrEqualKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetLessThan(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetLessThan(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetLessThan(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetLessThan, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetLessThanKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetLessThanKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetLessThanOrEqual(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetLessThanOrEqual(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void GetLessThanOrEqual(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetLessThanOrEqual, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetLessThanOrEqualKey(PositionBlock positionBlock, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetLessThanOrEqualKey, positionBlock, null, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public uint GetPosition(PositionBlock positionBlock) {
            var dataBuffer = new byte[4];
            this.GetPosition(positionBlock, dataBuffer);
            return dataBuffer.GetUInt32();
        }

        public void GetPosition(PositionBlock positionBlock, byte[] dataBuffer) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            if (dataBuffer.Length < 4) {
                throw new ArgumentException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.GetPosition, positionBlock, dataBuffer, null, keyNumber);
        }

        public void GetDirect(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.GetDirect, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, (ushort)lockBias);
        }

        public void GetDirect(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer, sbyte keyNumber, LockBias lockBias = LockBias.None) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.GetDirect(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber, lockBias);
        }

        public void Insert(PositionBlock positionBlock, byte[] dataBuffer, sbyte keyNumber = -1, byte keyLength = 255) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.Insert(positionBlock, dataBuffer, (ushort)dataBuffer.Length, keyNumber, keyLength);
        }

        public void Insert(PositionBlock positionBlock, byte[] dataBuffer, ushort dataBufferLength, sbyte keyNumber = -1, byte keyLength = 255) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var keyBuffer = keyNumber == -1 ? _zero : new byte[keyLength];
            this.Operate(Operation.Insert, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber);
        }

        public void InsertExtended(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer = null, sbyte keyNumber = -1) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.InsertExtended(positionBlock, dataBuffer, dataBufferLength, keyBuffer, keyNumber);
        }

        public void InsertExtended(PositionBlock positionBlock, byte[] dataBuffer, ushort dataBufferLength, byte[] keyBuffer = null, sbyte keyNumber = -1) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.InsertExtended, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber);
        }

        public void Update(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer = null, sbyte keyNumber = -1) {
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.Update(positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber);
        }

        public void Update(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer = null, sbyte keyNumber = -1) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.Update, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber);
        }

        public void UpdateChunk(PositionBlock positionBlock, byte[] dataBuffer, byte[] keyBuffer = null, sbyte keyNumber = -1) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            var dataBufferLength = (ushort)dataBuffer.Length;
            this.Operate(Operation.UpdateChunk, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber);
        }

        public void UpdateChunk(PositionBlock positionBlock, byte[] dataBuffer, ref ushort dataBufferLength, byte[] keyBuffer = null, sbyte keyNumber = -1) {
            if (positionBlock == null || dataBuffer == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.UpdateChunk, positionBlock, dataBuffer, ref dataBufferLength, keyBuffer, keyNumber);
        }

        public void Delete(PositionBlock positionBlock) {
            if (positionBlock == null) {
                throw new ArgumentNullException();
            }
            var keyNumber = (sbyte)0;
            this.Operate(Operation.Delete, positionBlock, null, null, keyNumber);
        }

        public void UnlockSingleRecord(PositionBlock positionBlock, sbyte keyNumber) {
            if (positionBlock == null) {
                throw new ArgumentNullException();
            }
            this.Operate(Operation.Unlock, positionBlock, null, null, keyNumber);
        }

        public void UnlockMultiRecord(PositionBlock positionBlock, byte[] recordPosition = null, bool isBufferReused = true) {
            if (positionBlock == null) {
                throw new ArgumentNullException();
            }
            if (recordPosition.Length != 4) {
                throw new ArgumentException();
            }
            var dataBuffer = recordPosition;
            var keyNumber = recordPosition == null ? (sbyte)-2 : (sbyte)-1;
            this.Operate(Operation.Unlock, positionBlock, dataBuffer, null, keyNumber);
        }

        public void BeginTransaction(TransactionMode transactionMode = TransactionMode.Concurrency, LockBias lockBias = LockBias.None) {
            var keyNumber = (sbyte)0;
            var bias = (ushort)((ushort)transactionMode + (ushort)lockBias);
            this.Operate(Operation.BeginTransaction, null, null, null, keyNumber, bias);
        }

        public void AbortTransaction() {
            var keyNumber = (sbyte)0;
            this.Operate(Operation.AbortTransaction, null, null, null, keyNumber);
        }

        public void EndTransaction() {
            var keyNumber = (sbyte)0;
            this.Operate(Operation.EndTransaction, null, null, null, keyNumber);
        }

        public void Reset() {
            var keyNumber = (sbyte)0;
            this.Operate(Operation.Reset, null, null, null, keyNumber);
        }

        public void Stop() {
            var keyNumber = (sbyte)0;
            this.Operate(Operation.Stop, null, null, null, keyNumber);
        }

        public IEnumerable<VersionInfo> Version() {
            var dataBuffer = new byte[15];
            var keyNumber = (sbyte)0;
            this.Operate(Operation.Version, null, dataBuffer, null, keyNumber);
            for (var i = 0; i < 3; i++) {
                var version = new VersionInfo(dataBuffer, i * 5);
                if (version.Type != 0) {
                    yield return version;
                }
            }
        }

        public void SetDirectory(string path) {
            if (path == null) {
                throw new ArgumentNullException();
            }
            path = path.TrimEnd('\0') + "\0";
            var keyBuffer = (this.PathEncoding ?? Encoding.Default).GetBytes(path);
            var keyNumber = (sbyte)0;
            this.Operate(Operation.SetDirectory, null, null, keyBuffer, keyNumber);
        }

        public string GetDirectory(sbyte driveNumber = 0) {
            var keyBuffer = new byte[255];
            var keyNumber = driveNumber;
            this.Operate(Operation.GetDirectory, null, null, keyBuffer, keyNumber);

            var path = this.PathEncoding.GetString(keyBuffer);
            return path.Substring(0, path.IndexOf('\0'));
        }

    }
}
