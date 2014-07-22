using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    enum RecordStateTransitions : byte
    {
        Save,
        Delete,
        Modify,
        Attach,
        Detach
    }
}
