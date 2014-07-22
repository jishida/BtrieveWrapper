using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class PositionBlock
    {
        public PositionBlock() {
            this.Value = new byte[128];
        }

        internal byte[] Value { get; private set; }
    }
}
