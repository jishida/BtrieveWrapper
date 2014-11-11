using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Utilities;

namespace BtrieveWrapper
{
    public class VersionInfo
    {
        internal VersionInfo(byte[] dataBuffer, int offset) {
            this.VersionNo = dataBuffer.GetUInt16(offset + 0);
            this.RevisionNo = dataBuffer.GetUInt16(offset + 2);
            this.Type = dataBuffer[offset + 4];
        }

        public ushort VersionNo { get; private set; }
        public ushort RevisionNo { get; private set; }
        public byte Type { get; private set; }
    }
}
