using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public abstract class TransactionalObjectFactory : IDisposable
    {
        List<ITransactionalObject> _managedObjects;

        protected TransactionalObjectFactory(Operator nativeOperator) {
            this.Operator = nativeOperator;
            this.Transaction = null;
            _managedObjects = new List<ITransactionalObject>();
        }

        protected Operator Operator { get; private set; }
        protected Transaction Transaction { get; private set; }
        protected IEnumerable<object> ManagedObjects { get { return _managedObjects.Select(o => (object)o); } }

		public Encoding PathEncoding{ get { return this.Operator.PathEncoding; } set { this.Operator.PathEncoding = value; } }
		public Encoding OwnerNameEncoding{ get { return this.Operator.OwnerNameEncoding; } set { this.Operator.OwnerNameEncoding = value; } }

        protected void AddTransactionalObject(object obj) {
            var transactionalObject = (ITransactionalObject)obj;
            _managedObjects.Add(transactionalObject);
            transactionalObject.Disposing += OnTransactionalObjectDisposing;
            if (this.Transaction != null) {
                transactionalObject.SetTransaction(this.Transaction);
            }
        }

        void OnTransactionalObjectDisposing(object sender, EventArgs e) {
            var obj = (ITransactionalObject)sender;
            _managedObjects.Remove(obj);
            obj.Disposing -= OnTransactionalObjectDisposing;
        }

        void OnTransactionDisposing(object sender, EventArgs e) {
            this.Transaction.Disposing -= OnTransactionDisposing;
            this.Transaction = null;
        }

        public virtual Transaction BeginTransaction(TransactionMode transactionMode = TransactionMode.Concurrency, LockMode lockMode = LockMode.None) {
            if (this.Transaction != null) {
                throw new InvalidOperationException();
            }
            this.Transaction = new Transaction(this.Operator, transactionMode, lockMode);
            this.Transaction.Disposing += OnTransactionDisposing;
            foreach (var managedObject in _managedObjects) {
                managedObject.SetTransaction(this.Transaction);
            }
            return this.Transaction;
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (this.Transaction != null) {
                    this.Transaction.Dispose();
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
