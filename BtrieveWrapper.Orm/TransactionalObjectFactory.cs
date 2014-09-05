using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public abstract class TransactionalObjectFactory : IDisposable
    {
        List<ITransactionalObject> _managedObjects;

        protected TransactionalObjectFactory(NativeOperator nativeOperator) {
            this.Operator = nativeOperator;
            this.Transaction = null;
            _managedObjects = new List<ITransactionalObject>();
        }

        protected NativeOperator Operator { get; private set; }
        public Transaction Transaction { get; private set; }
        protected IEnumerable<object> ManagedObjects { get { return _managedObjects.Select(o => (object)o); } }

		public Encoding PathEncoding{ get { return this.Operator.PathEncoding; } set { this.Operator.PathEncoding = value; } }
		public Encoding OwnerNameEncoding{ get { return this.Operator.OwnerNameEncoding; } set { this.Operator.OwnerNameEncoding = value; } }

        protected void AddTransactionalObject(object obj) {
            var transactionalObject = obj as ITransactionalObject;
            _managedObjects.Add(transactionalObject);
            transactionalObject.Disposing += OnTransactionalObjectDisposing;
            transactionalObject.SetFactory(this);
        }

        void OnTransactionalObjectDisposing(object sender, EventArgs e) {
            var obj = (ITransactionalObject)sender;
            _managedObjects.Remove(obj);
            obj.SetFactory(null);
            obj.Disposing -= OnTransactionalObjectDisposing;
        }

        void OnTransactionDisposing(object sender, EventArgs e) {
            this.Transaction.Disposing -= OnTransactionDisposing;
            this.Transaction = null;
        }

        void OnTransactionCommitted(object sender, EventArgs e) {
            foreach (var managedObject in _managedObjects) {
                managedObject.TransactionCommitted();
            }
        }

        void OnTransactionRollbacked(object sender, EventArgs e) {
            foreach (var managedObject in _managedObjects) {
                managedObject.TransactionRollbacked();
            }
        }

        public virtual Transaction BeginTransaction(TransactionMode transactionMode = TransactionMode.Concurrency, LockMode lockMode = LockMode.None) {
            if (this.Transaction != null) {
                throw new InvalidOperationException();
            }
            this.Transaction = new Transaction(this.Operator, transactionMode, lockMode);
            this.Transaction.Disposing += OnTransactionDisposing;
            this.Transaction.Committed += OnTransactionCommitted;
            this.Transaction.Rollbacked += OnTransactionRollbacked;

            return this.Transaction;
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (this.Transaction != null) {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
                foreach (ITransactionalObject mabagedObject in this.ManagedObjects) {
                    mabagedObject.Dispose();
                }
            }
        }

        public void Dispose() {
            this.Dispose(true);
        }

    }
}
