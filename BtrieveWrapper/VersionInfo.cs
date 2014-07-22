using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class VersionInfo
    {
        internal VersionInfo(byte[] dataBuffer, int offset) {
            this.VersionNo = BitConverter.ToUInt16(dataBuffer, offset + 0);
            this.RevisionNo = BitConverter.ToUInt16(dataBuffer, offset + 2);
            this.Type = dataBuffer[offset + 4];
        }

        public ushort VersionNo { get; private set; }
        public ushort RevisionNo { get; private set; }
        public byte Type { get; private set; }
    }
}
