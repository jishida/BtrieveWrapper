using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    interface ITransactionalObject : IDisposable
    {
        void SetTransaction(Transaction transaction);
        event EventHandler Disposing;
    }
}
