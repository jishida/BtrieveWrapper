using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    interface ITransactionalObject : IDisposable
    {
        void SetFactory(TransactionalObjectFactory factory);
        void TransactionCommitted();
        void TransactionRollbacked();
        event EventHandler Disposing;
    }
}
