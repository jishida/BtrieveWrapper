using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public enum FileFlag : ushort
    {
        None = 0,
        VarRecs = 1,
        BlankTrunc = 2,
        PreAlloc = 4,
        DataComp = 8,
        KeyOnly = 16,
        BalancedKeys = 32,
        Free10 = 64,
        Free20 = 128,
        Free30 = 192,
        DupPtrs = 256,
        SystemDataIncluded = 512,
        NoSystemDataIncluded = 4608,
        SpecifyKeyNums = 1024,
        VatsSupport = 2048
    }
}
