using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class RecordReader<TRecord, TKeyCollection> : ITransactionalObject
        where TRecord : Record<TRecord>
        where TKeyCollection : KeyCollection<TRecord>, new()
    {
        protected sealed class Connection : IDisposable
        {
            RecordReader<TRecord, TKeyCollection> _manager = null;

            Connection() { }

            public Connection(RecordReader<TRecord, TKeyCollection> manager) {
                if (manager._connection == null) {
                    if (manager.CheckTransaction()) {
                        manager._connection = new Connection();
                        manager._connection._manager = manager;
                    }else{
                        _manager = manager;
                    }
                    if (!manager.Operator.IsOpened) {
                        manager.Operator.Open();
                        if (manager._stat == null) {
                            manager._stat = manager.Operator.Stat;
                            ValidateStat(manager);
                        }
                    }
                }
            }

            static void ValidateStat(RecordReader<TRecord, TKeyCollection> manager) {
                var keySpecs = new List<KeySpec>();
                foreach (var keySpec in manager._stat.KeySpecs) {
                    keySpecs.Add(keySpec);
                    if (!keySpec.IsSegmentKey) {
                        var fields = manager.Operator.RecordInfo.Fields.Where(f => f.KeySegments.Any(a => a.KeyNumber == keySpec.Number));
                        if (fields.Count() != keySpecs.Count()) {
                            throw new InvalidDefinitionException();
                        }
                        foreach (var field in fields) {
                            var tmpKeySpec = keySpecs.SingleOrDefault(s => s.Position == field.Position && s.Length == field.Length);
                            if (tmpKeySpec == null) {
                                throw new InvalidDefinitionException();
                            } else {
                                keySpecs.Remove(tmpKeySpec);
                            }
                        }
                        if (keySpecs.Count != 0) {
                            throw new InvalidDefinitionException();
                        }
                    }
                }
                if (keySpecs.Count != 0) {
                    throw new InvalidDefinitionException();
                }
            }

            public void Dispose() {
                if (_manager != null) {
                    _manager.Operator.IsClosable = true;
                    try {
                        _manager.Operator.Close();
                    } finally {
                        _manager.Operator.IsClosable = false;
                    }
                }
            }
        }

        Connection _connection = null;
        StatData _stat;

        public RecordReader(
            Path path = null,
            string ownerName = null,
            OpenMode? openMode = null,
            string applicationId = "BW",
            ushort threadId = 0,
            string dllPath = null,
            IEnumerable<string> dependencyPaths = null,
            int reusableCapacity = 1000,
            byte[] temporaryBuffer = null) {

            this.Keys = new TKeyCollection();

            this.Operator = new RecordOperator<TRecord>(applicationId, threadId, path, ownerName, openMode, dllPath, dependencyPaths, reusableCapacity, temporaryBuffer);
            this.Operator.IsClosable = false;
            this.PrimaryKey = this.Keys[this.Operator.PrimaryKeyNumber];
            _stat = this.Operator.Stat;
        }

        public RecordReader(
            string path,
            string ownerName = null,
            OpenMode? openMode = null,
            string applicationId = "BW",
            ushort threadId = 0,
            string dllPath = null,
            IEnumerable<string> dependencyPaths = null,
            int reusableCapacity = 1000,
            byte[] temporaryBuffer = null)
            : this(Path.Absolute(path), ownerName, openMode, applicationId, threadId, dllPath, dependencyPaths, reusableCapacity, temporaryBuffer) { }

        public RecordReader(
            NativeOperator nativeOperator,
            Path path = null,
            string ownerName = null,
            OpenMode? openMode = null,
            int reusableCapacity = 1000,
            byte[] temporaryBuffer = null) {

            if (nativeOperator == null) {
                throw new ArgumentNullException();
            }

            this.Keys = new TKeyCollection();

            this.Operator = new RecordOperator<TRecord>(nativeOperator, path, ownerName, openMode, reusableCapacity, temporaryBuffer);
            this.Operator.IsClosable = false;
            this.PrimaryKey = this.Keys[this.Operator.PrimaryKeyNumber];
            _stat = this.Operator.Stat;
        }

        public RecordReader(
            NativeOperator nativeOperator,
            string path,
            string ownerName = null,
            OpenMode? openMode = null,
            int reusableCapacity = 1000,
            byte[] temporaryBuffer = null)
            : this(nativeOperator, Path.Absolute(path), ownerName, openMode, reusableCapacity, temporaryBuffer) { }

        public RecordOperator<TRecord> Operator { get; protected set; }
        public KeyInfo PrimaryKey { get; private set; }
        public TKeyCollection Keys { get; private set; }
        public bool AutoUnlockUnhandledRecord { get; private set; }
        public bool IsOpened { get { return _connection != null; } }
        protected TransactionalObjectFactory Factory { get; private set; }

        protected bool CheckTransaction() {
            return this.Factory != null && this.Factory.Transaction != null;
        }

        void UnlockLastRecord(LockMode lockMode) {
            switch (lockMode) {
                case LockMode.WaitLock:
                case LockMode.NoWaitLock:
                    var position = this.Operator.GetPosition();
                    this.Operator.UnlockMultiRecord(position);
                    break;
                case LockMode.None:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void Unlock(TRecord record) {
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (record.RecordState != RecordState.Unchanged) {
                throw new ArgumentException();
            }
            if (_connection == null) {
                throw new InvalidOperationException();
            }
            this.Operator.GetEqual(record.GetKeyValue(this.PrimaryKey), overwrite: true);
            var position = this.Operator.GetPosition();
            this.Operator.UnlockMultiRecord(position);
        }

        public void UnlockAllRecords() {
            if (_connection == null) {
                throw new InvalidOperationException();
            }
            this.Operator.UnlockMultiRecord();
        }

        public TRecord Get(System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null, LockMode lockMode = LockMode.None) {
            using (var connection = new Connection(this)) {
                TRecord result = null;
                foreach (var record in this.Query(new QueryParameter<TRecord>(whereExpression, lockMode: lockMode, limit: 2))) {
                    if (result != null) {
                        throw new ArgumentException();
                    }
                    result = record;
                }
                return result;
            }
        }

        public TRecord Get(System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression, KeyInfo key, LockMode lockMode = LockMode.None) {
            using (var connection = new Connection(this)) {
                TRecord result = null;
                foreach (var record in this.Query(new QueryParameter<TRecord>(key, whereExpression, lockMode: lockMode, limit: 2))) {
                    if (result != null) {
                        throw new ArgumentException();
                    }
                    result = record;
                }
                return result;
            }
        }

        public TRecord Get(System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression, Func<TKeyCollection, KeyInfo> keySelector, LockMode lockMode = LockMode.None) {
            return this.Get(whereExpression, keySelector == null ? null : keySelector(this.Keys), lockMode);
        }

        public TRecord Get(KeyValue keyValue, LockMode lockMode = LockMode.None) {
            if (keyValue == null) {
                throw new ArgumentNullException();
            }
            if (keyValue.Key.DuplicateKeyOption != DuplicateKeyOption.Unique || keyValue.ComplementCount != 0) {
                throw new ArgumentException();
            }
            var lockBias = Utility.GetLockBias(this.CheckTransaction() ? this.Factory.Transaction.LockMode : lockMode);
            TRecord result;
            using (var connector = new Connection(this)) {
                try {
                    result = this.Operator.GetEqual(keyValue.DeepCopy(), lockBias, true);
                } catch {
                    return null;
                }
            }
            return result;
        }

        public IEnumerable<TRecord> Query(
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression,
            Func<TKeyCollection, KeyInfo> keySelector,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false) {

            return this.Query(new QueryParameter<TRecord>(
                keySelector == null ? null : keySelector(this.Keys),
                whereExpression,
                lockMode,
                startingRecord,
                skipStartingRecord,
                limit,
                reverse,
                rejectCount,
                isIgnoreCase));
        }

        public IEnumerable<TRecord> Query(
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression,
            KeyInfo key,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false) {

            return this.Query(new QueryParameter<TRecord>(
                key,
                whereExpression,
                lockMode,
                startingRecord,
                skipStartingRecord,
                limit,
                reverse,
                rejectCount,
                isIgnoreCase));
        }

        public IEnumerable<TRecord> Query(
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false) {
            
            return this.Query(new QueryParameter<TRecord>(
                whereExpression,
                lockMode,
                startingRecord,
                skipStartingRecord,
                limit,
                reverse,
                rejectCount,
                isIgnoreCase));
        }

        public IEnumerable<TRecord> Query(QueryParameter<TRecord> parameter) {
            if (parameter == null) {
                throw new ArgumentNullException();
            }
            if (parameter.StartingRecord != null && parameter.StartingRecord.RecordState == RecordState.Incomplete) {
                throw new ArgumentException();
            }
            var lockMode = this.CheckTransaction() ? this.Factory.Transaction.LockMode : parameter.LockMode;
            var lockBias = Utility.GetLockBias(lockMode);
            var unlock = _connection != null && this.AutoUnlockUnhandledRecord;
            KeyValue keyValue = parameter.UniqueKeyValue;
            if (keyValue != null) {
                using (var connection = new Connection(this)) {
                    TRecord record;
                    try {
                        record = this.Operator.GetEqual(keyValue, lockBias, true);
                    } catch (OperationException e) {
                        if (e.StatusCode == 4) {
                            yield break;
                        }
                        throw;
                    }
                    if (parameter.Filter == null || parameter.Filter(record)) {
                        yield return record;
                    } else {
                        this.Operator.Recycle(record);
                        if (unlock) {
                            this.UnlockLastRecord(lockMode);
                        }
                    }
                    yield break;
                }
            }

            var limit = parameter.Limit;
            keyValue = parameter.StartingRecord == null
                ? null : parameter.StartingRecord.GetKeyValue(this.PrimaryKey);
            using (var connection = new Connection(this)) {
                if (keyValue == null) {
                    TRecord record;
                    try {
                        if (parameter.UseKey) {
                            record = parameter.Reverse
                                ? this.Operator.GetLast(parameter.Key, lockBias)
                                : this.Operator.GetFirst(parameter.Key, lockBias);
                            keyValue = record.GetKeyValue(parameter.Key);
                        } else {
                            record = parameter.Reverse
                                ? this.Operator.StepLast(lockBias)
                                : this.Operator.StepFirst(lockBias);
                        }
                    } catch (OperationException e) {
                        if (e.StatusCode == 9) {
                            yield break;
                        }
                        throw;
                    }
                } else {
                    var tryGreater = false;
                    try {
                        this.Operator.GetEqual(keyValue, lockBias, true);
                    } catch (OperationException e) {
                        if (e.StatusCode == 4 && parameter.UseKey) {
                            tryGreater = true;
                        } else {
                            throw;
                        }
                    }
					if (tryGreater) {
						keyValue = parameter.StartingRecord.GetKeyValue (parameter.Key);
						try {
							if (parameter.Reverse) {
								this.Operator.GetLessThan (keyValue, lockBias, true);
							} else {
								this.Operator.GetGreater (keyValue, lockBias, true);
							}
						} catch (OperationException e) {
							if (e.StatusCode == 9) {
								yield break;
							}
							throw;
						}
					}
                }
                var records = parameter.UseKey
                    ? parameter.Reverse
                        ? this.Operator.GetPreviousExtended(keyValue, parameter.ApiFilter, false, limit, lockBias, parameter.RejectCount, true)
                        : this.Operator.GetNextExtended(keyValue, parameter.ApiFilter, false, limit, lockBias, parameter.RejectCount, true)
                    : parameter.Reverse
                        ? this.Operator.StepPreviousExtended(parameter.ApiFilter, false, limit, lockBias, parameter.RejectCount)
                        : this.Operator.StepNextExtended(parameter.ApiFilter, false, limit, lockBias, parameter.RejectCount);
                var skip = parameter.SkipStartingRecord;
                foreach (var record in records) {
                    if (parameter.AdditionalFilter == null || parameter.AdditionalFilter(record)) {
                        if (skip) {
                            skip = false;
                        } else {
                            yield return record;
                        }
                    } else {
                        this.Operator.Recycle(record);
                        if (unlock) {
                            this.UnlockLastRecord(lockMode);
                        }
                    }
                }
            }
        }

        void ITransactionalObject.SetFactory(TransactionalObjectFactory factory) {
            this.Factory = factory;
        }

        void ITransactionalObject.TransactionCommitted() {
            this.OnTransactionCommitted();
        }

        void ITransactionalObject.TransactionRollbacked() {
            this.OnTransactionRollbacked();
        }

        protected virtual void OnTransactionCommitted() { }

        protected virtual void OnTransactionRollbacked() { }

        public void Open() {
            if (_connection == null) {
                _connection = new Connection(this);
            }
        }

        public void Close() {
            if (_connection != null) {
                _connection.Dispose();
                _connection = null;
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (this.Disposing != null) {
                    this.Disposing(this, null);
                }
                this.Close();
                this.Operator.Dispose();
                this.Operator = null;
            }
        }

        public void Dispose() {
            this.Dispose(true);
        }

        public event EventHandler Disposing = null;
    }
}
