using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class RecordManager<TRecord> : RecordReader<TRecord>, IRecordManager where TRecord : Record<TRecord>
    {
        List<TRecord> _managedRecords;
        List<TRecord> _rollbackedRecords;

        public RecordManager(
            Path path = null,
            string dllPath = null,
            string applicationId = "BW",
            ushort threadId = 0,
            string ownerName = null,
            OpenMode? openMode = null,
            int reusableCapacity = 1000,
            byte[] temporaryBuffer = null)
            : base(path, dllPath, applicationId, threadId, ownerName, openMode, reusableCapacity, temporaryBuffer) {

            _managedRecords = new List<TRecord>();
            _rollbackedRecords = new List<TRecord>();
        }

        public RecordManager(
            Operator nativeOperator,
            Path path = null,
            string ownerName = null,
            OpenMode? openMode = null,
            int reusableCapacity = 1000,
            byte[] temporaryBuffer = null)
            : base(nativeOperator, path, ownerName, openMode, reusableCapacity, temporaryBuffer) {

            _managedRecords = new List<TRecord>();
            _rollbackedRecords = new List<TRecord>();
        }

        public void Detach(TRecord record) {
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (record.IsManagedMember) {
                record.IsManagedMember = false;
                record.ChangeState(RecordStateTransitions.Detach);
                _managedRecords.Remove(record);
            } else if (record.IsRollbackedMember) {
                record.IsRollbackedMember = false;
                record.ChangeState(RecordStateTransitions.Detach);
                _rollbackedRecords.Remove(record);
            } else {
                throw new ArgumentException();
            }
        }

        public void Save(TRecord record) {
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (!_managedRecords.Contains(record) || record.RecordState == RecordState.Detached) {
                throw new ArgumentException();
            }
            if (record.RecordState == RecordState.Unchanged) {
                return;
            }
            using (var connection = new Connection(this)) {
                switch (record.RecordState) {
                    case RecordState.Added:
                        try {
                            this.Operator.Insert(record);
                        } catch (OperationException e) {
                            if (e.StatusCode == 5) {
                                this.Operator.GetEqual(record.GetKeyValue(this.PrimaryKey), overwrite: true);
                                this.Operator.Update(record);
                            } else {
                                throw;
                            }
                        }
                        break;
                    case RecordState.Deleted:
                        try {
                            this.Operator.GetEqual(record.GetKeyValue(this.PrimaryKey), overwrite: true);
                            this.Operator.Delete();
                        } catch (OperationException e) {
                            if (e.StatusCode != 4) {
                                throw;
                            }
                        }
                        break;
                    case RecordState.Modified:
                        try {
                            this.Operator.GetEqual(record.GetKeyValue(this.PrimaryKey), overwrite: true);
                            this.Operator.Update(record);
                        } catch (OperationException e) {
                            if (e.StatusCode == 4) {
                                this.Operator.Insert(record);
                            } else {
                                throw;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException();
                }
                record.ChangeState(RecordStateTransitions.Save, this.Transaction != null);
                if (record.RecordState == RecordState.Detached) {
                    _managedRecords.Remove(record);
                    if (this.Transaction != null && record.RollbackedState != RecordState.Detached && !record.IsRollbackedMember) {
                        record.IsRollbackedMember = true;
                        _rollbackedRecords.Add(record);
                    }
                }
            }
        }

        public void Add(TRecord record) {
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (record.RecordState != RecordState.Detached) {
                throw new ArgumentException();
            }
            record.ChangeState(RecordStateTransitions.Attach);
            _managedRecords.Add(record);
            record.IsManagedMember = true;
            if (record.IsRollbackedMember) {
                record.IsRollbackedMember = false;
                _rollbackedRecords.Remove(record);
            }
        }

        public void Remove(TRecord record) {
            if (record == null) {
                throw new ArgumentNullException();
            }
            if (!record.IsManagedMember) {
                throw new ArgumentException();
            }
            record.ChangeState(RecordStateTransitions.Delete);
            if (record.RecordState == RecordState.Detached && record.IsManagedMember) {
                record.IsManagedMember = false;
                _managedRecords.Remove(record);
                if (this.Transaction != null && !record.IsRollbackedMember) {
                    record.IsRollbackedMember = true;
                    _rollbackedRecords.Add(record);
                }
            }
        }

        public TRecord GetAndManage(System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null, LockMode lockMode = LockMode.None) {
            var result = this.Get(null, whereExpression, lockMode);
            if (result != null) {
                this.ManageRecord(result);
            }
            return result;
        }

        public TRecord GetAndManage(KeyInfo key, System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null, LockMode lockMode = LockMode.None) {
            var result = this.Get(key, whereExpression, lockMode);
            if (result != null) {
                this.ManageRecord(result);
            }
            return result;
        }

        public TRecord GetAndManage(KeyValue keyValue, LockMode lockMode = LockMode.None) {
            var result = this.Get(keyValue, lockMode);
            if (result != null) {
                this.ManageRecord(result);
            }
            return result;
        }

        public IEnumerable<TRecord> QueryAndManage(
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            TRecord startingRecord = null,
            int limit = 0,
            LockMode lockMode = LockMode.None,
            bool skipStartingRecord = false,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false) {

            return this.QueryAndManage(new QueryParameter<TRecord>(whereExpression, startingRecord, lockMode, skipStartingRecord, limit, reverse, rejectCount, isIgnoreCase));
        }

        public IEnumerable<TRecord> QueryAndManage(
            KeyInfo key,
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            TRecord startingRecord = null,
            int limit = 0,
            LockMode lockMode = LockMode.None,
            bool skipStartingRecord = false,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false) {

            return this.QueryAndManage(new QueryParameter<TRecord>(key, whereExpression, startingRecord, lockMode, skipStartingRecord, limit, reverse, rejectCount, isIgnoreCase));
        }


        public IEnumerable<TRecord> QueryAndManage(QueryParameter<TRecord> parameter) {
            var count = 0;
            foreach (var record in this.Query(parameter)) {
                this.ManageRecord(record);
                yield return record;
                count++;
            }
        }

        void ManageRecord(TRecord record) {
            record.SetUnchanged();
            _managedRecords.Add(record);
            record.IsManagedMember = true;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                foreach (var record in _managedRecords) {
                    record.IsManagedMember = false;
                    record.ChangeState(RecordStateTransitions.Detach);
                }
            }
        }


        protected override void SetTransaction(Transaction transaction) {
            base.SetTransaction(transaction);
            this.Transaction.Committed += OnTransactionCommitted;
            this.Transaction.Rollbacked += OnTransactionRollbacked;
        }

        #region IRecordManager

        void OnTransactionCommitted(object sender, EventArgs e) {
            this.Transaction.Committed -= OnTransactionCommitted;
            foreach (var record in _managedRecords) {
                record.Commit();
            }
            _rollbackedRecords.Clear();
        }

        void OnTransactionRollbacked(object sender, EventArgs e) {
            this.Transaction.Rollbacked -= OnTransactionRollbacked;
            var detachedRecords = new List<TRecord>();
            foreach (var record in _managedRecords) {
                record.Rollback();
                if (record.RecordState == RecordState.Detached) {
                    detachedRecords.Add(record);
                }
            }
            foreach (var record in _rollbackedRecords) {
                record.Rollback();
                if (record.RecordState != RecordState.Detached) {
                    _managedRecords.Add(record);
                }
            }
            _rollbackedRecords.Clear();
            foreach (var record in detachedRecords) {
                _managedRecords.Remove(record);
            }
        }

        public void DetachAllRecords() {
            foreach (var record in _managedRecords) {
                record.IsManagedMember = false;
                record.ChangeState(RecordStateTransitions.Detach);
            }
            foreach (var record in _rollbackedRecords) {
                record.IsManagedMember = false;
                record.ChangeState(RecordStateTransitions.Detach);
            }
            _managedRecords.Clear();
            _rollbackedRecords.Clear();
        }

        public void SaveChanges(bool detachAllRecordsAfterSave = false, LockMode lockMode = LockMode.WaitLock) {
            var lockBias = Utility.GetLockBias(lockMode);
            var deletedRecords = _managedRecords.Where(r => r.RecordState == RecordState.Deleted).ToArray();
            var addedRecords = _managedRecords.Where(r => r.RecordState == RecordState.Added);
            var modifiedRecords = _managedRecords.Where(r => r.RecordState == RecordState.Modified);
            if (deletedRecords.Count() == 0 && addedRecords.Count() == 0 && modifiedRecords.Count() == 0) {
                return;
            }
            using (var connection = new Connection(this)) {
                foreach (var record in deletedRecords) {
                    if (record.HasPhysicalPosition) {
                        this.Operator.SetPosition(record.PhysicalPosition, lockBias: lockBias);
                    } else {
                        this.Operator.GetEqual(record.GetKeyValue(this.PrimaryKey), lockBias, true);
                    }
                    this.Operator.Delete();
                    record.ChangeState(RecordStateTransitions.Save, this.Transaction != null);
                    record.IsManagedMember = false;
                    _managedRecords.Remove(record);
                    if (this.Transaction != null && record.RollbackedState != RecordState.Detached && !record.IsRollbackedMember) {
                        record.IsRollbackedMember = true;
                        _rollbackedRecords.Add(record);
                    }
                }
                this.Operator.InsertExtended(addedRecords, OnInserted);
                foreach (var record in modifiedRecords) {
                    if (record.HasPhysicalPosition) {
                        this.Operator.SetPosition(record.PhysicalPosition, lockBias: lockBias);
                    } else {
                        this.Operator.GetEqual(record.GetKeyValue(this.PrimaryKey), lockBias, true);
                    }
                    this.Operator.Update(record);
                    record.ChangeState(RecordStateTransitions.Save, this.Transaction != null);
                }
            }
            if (detachAllRecordsAfterSave) {
                this.DetachAllRecords();
            }
        }

        void OnInserted(IEnumerable<TRecord> records) {
            foreach (var record in records) {
                record.ChangeState(RecordStateTransitions.Save, this.Transaction != null);
            }
        }

        #endregion

    }
}
