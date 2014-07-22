using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class AcsIdentifier
    {
        AcsIdentifier(AcsType type) {
            this.Type = type;
        }

        public AcsType Type { get; private set; }

        internal void SetDataBuffer(byte[] dataBuffer, ushort position){
            throw new NotImplementedException();
        }
    }
}
