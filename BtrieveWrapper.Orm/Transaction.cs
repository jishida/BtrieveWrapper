using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public sealed class Transaction : IDisposable
    {

        string _clientId = null;
        ushort _threadId;
        bool _disposed = false, _committed = false;
        Operator _operator;

        internal Transaction(Operator nativeOperator , TransactionMode transactionMode, LockMode lockMode) {
            _operator = nativeOperator;
            this.TransactionMode = transactionMode;
            this.LockMode = lockMode;
            _operator.BeginTransaction(transactionMode, Utility.GetLockBias(lockMode));
            if (_operator.ClientId != null) {
                _clientId = _operator.ClientId.ApplicationId;
                _threadId = _operator.ClientId.ThreadId;
            }
        }

        public TransactionMode TransactionMode { get; private set; }
        public LockMode LockMode { get; private set; }

        public event EventHandler Committing = null;
        public event EventHandler Committed = null;
        public event EventHandler Rollbacking = null;
        public event EventHandler Rollbacked = null;
        public event EventHandler Disposing = null;

        public void Commit(bool detachAllRecordsAfterCommit = false) {
            if (_disposed) {
                throw new InvalidOperationException();
            }
            if (this.Committing != null) {
                this.Committing(this, null);
            }
            try {
                _operator.EndTransaction();
                _committed = true;
                if (this.Committed != null) {
                    this.Committed(this, null);
                }
            } finally {
                this.Dispose();
            }
        }

        void Dispose(bool disposing) {
            if (disposing) {
                if (!_disposed) {
                    if (!_committed && this.Rollbacking != null) {
                        this.Rollbacking(this, null);
                    }
                    try {
                        _operator.AbortTransaction();
                        if (!_committed && this.Rollbacked != null) {
                            this.Rollbacked(this, null);
                        }
                    } catch (OperationException e) {
                        if (e.StatusCode != 39) {
                            throw;
                        }
                    } finally {
                        _disposed = true;
                        if (this.Disposing != null) {
                            this.Disposing(this, null);
                        }
                    }
                }
            }
        }

        public void Dispose() {
            this.Dispose(true);
        }
    }
}
