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
        Dictionary<Type, Func<Operator, Path, string, OpenMode?, int, byte[], ITransactionalObject>> _managerConstructorDictionary
            = new Dictionary<Type, Func<Operator, Path, string, OpenMode?, int, byte[], ITransactionalObject>>();

        protected DbClient(string dllPath = null, string applicationId = "BW")
            : base(new Operator(applicationId, Resource.GetThreadId(), dllPath)) { }

        protected DbClient(INativeLibrary nativeLibrary, string applicationId = "BW")
            : base(new Operator(applicationId, Resource.GetThreadId(), nativeLibrary)) { }

        static bool CheckManagerType(Type managerType) {
            while (managerType != typeof(object)) {
                if (managerType.BaseType.IsGenericType && managerType.BaseType.GetGenericTypeDefinition() == typeof(RecordManager<>)) {
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

        public object CreateManager(Type managerType, Path path = null, string ownerName = null, OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            if (managerType == null) {
                throw new ArgumentNullException();
            }
            if (!_managerConstructorDictionary.ContainsKey(managerType)) {
                if (!CheckManagerType(managerType)) {
                    throw new ArgumentException();
                }
                var constructorInfo = managerType.GetConstructor(new[] { typeof(Operator), typeof(Path), typeof(string), typeof(OpenMode?), typeof(int), typeof(byte[]) });
                if (constructorInfo == null) {
                    throw new InvalidDefinitionException();
                }
                var parameters = new[] { 
                    Expression.Parameter(typeof(Operator)), 
                    Expression.Parameter(typeof(Path)), 
                    Expression.Parameter(typeof(string)), 
                    Expression.Parameter(typeof(OpenMode?)), 
                    Expression.Parameter(typeof(int)), 
                    Expression.Parameter(typeof(byte[])) };
                var newExpression = Expression.New(constructorInfo, parameters);
                var lambda = Expression.Lambda<Func<Operator, Path, string, OpenMode?, int, byte[], ITransactionalObject>>(newExpression, parameters);
                _managerConstructorDictionary[managerType] = lambda.Compile();
            }
            var constructor = _managerConstructorDictionary[managerType];
            var temporaryBuffer = _temporaryBufferDictionary.ContainsKey(temporaryBufferId) ? _temporaryBufferDictionary[temporaryBufferId] : Resource.GetBuffer();
            var result = constructor(this.Operator, path, ownerName, openMode, recycleCount, temporaryBuffer);
            this.AddTransactionalObject(result);
            return result;
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
