using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class DbClient : TransactionalObjectFactory
    {
        Dictionary<int, byte[]> _temporaryBufferDictionary = new Dictionary<int, byte[]>();
        Dictionary<Type, Func<NativeOperator, Path, string, OpenMode?, int, byte[], ITransactionalObject>> _managerConstructorDictionary
            = new Dictionary<Type, Func<NativeOperator, Path, string, OpenMode?, int, byte[], ITransactionalObject>>();

        protected DbClient(string applicationId = "BW", string dllPath = null, IEnumerable<string> dependencyPaths = null)
            : base(new NativeOperator(applicationId, Resource.GetThreadId(), dllPath, dependencyPaths)) { }

        protected DbClient(INativeLibrary nativeLibrary, string applicationId = "BW")
            : base(new NativeOperator(applicationId, Resource.GetThreadId(), nativeLibrary)) { }

        static bool CheckManagerType(Type managerType) {
            while (managerType != typeof(object)) {
                if (managerType.BaseType.IsGenericType && managerType.BaseType.GetGenericTypeDefinition() == typeof(RecordManager<,>)) {
                    return true;
                }
                managerType = managerType.BaseType;
            }
            return false;
        }

        public override Transaction BeginTransaction(TransactionMode transactionMode = TransactionMode.Concurrency, LockMode lockMode = LockMode.None) {
            var result = base.BeginTransaction(transactionMode, lockMode);
            result.Committing += OnTransactionCommitting;
            return result;
        }

        void OnTransactionCommitting(object sender, EventArgs e) {
            this.SaveChanges();
            this.Transaction.Committing -= OnTransactionCommitting;
        }

        public object CreateManager<TRecord, TKeyCollection>(Path path = null, string ownerName = null, OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {
            var temporaryBuffer = _temporaryBufferDictionary.ContainsKey(temporaryBufferId) ? _temporaryBufferDictionary[temporaryBufferId] : Resource.GetBuffer();
            var result = new RecordManager<TRecord, TKeyCollection>(this.Operator, path, ownerName, openMode, recycleCount, temporaryBuffer);
            this.AddTransactionalObject(result);
            return result;
        }

        public object CreateManager<TRecord, TKeyCollection>(string path, string ownerName = null, OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {
            return this.CreateManager<TRecord, TKeyCollection>(Path.Absolute(path), ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public void SaveChanges(bool detachAllRecordsAfterSave = false, LockMode lockMode = LockMode.WaitLock) {
            foreach (IRecordManager manager in this.ManagedObjects) {
                manager.SaveChanges(detachAllRecordsAfterSave, lockMode);
            }
        }

        public void DetatchAllRecords() {
            foreach (IRecordManager manager in this.ManagedObjects) {
                manager.DetachAllRecords();
            }
        }

        public void Stop() {
            this.Reset();
            this.Operator.Stop();
        }

        public void Reset() {
            if (this.Transaction != null) {
                this.Transaction.Dispose();
            }
            this.Operator.Reset();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                this.Reset();
                Resource.RemoveThreadId(this.Operator.ClientId.ThreadId);
                foreach (var buffer in _temporaryBufferDictionary.Values) {
                    Resource.Recycle(buffer);
                }
            }
            base.Dispose(disposing);
        }
    }
}
