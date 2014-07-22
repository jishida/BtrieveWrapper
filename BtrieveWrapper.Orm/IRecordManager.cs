using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    interface IRecordManager
    {
        void DetachAllRecords();
        void SaveChanges(bool detachAllRecordsAfterSave, LockMode lockMode);
    }
}
