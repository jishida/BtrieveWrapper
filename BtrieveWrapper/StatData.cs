using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Utilities;

namespace BtrieveWrapper
{
    public class StatData
    {
        internal StatData(byte[] dataBuffer) {
            this.FileSpec = new StatFileSpec(
                dataBuffer.GetUInt16(0),
                dataBuffer.GetUInt16(2),
                dataBuffer[4],
                dataBuffer[5],
                dataBuffer.GetUInt32(6),
                (FileFlag)dataBuffer.GetUInt16(10),
                dataBuffer[12],
                dataBuffer.GetUInt16(14));
            var keySpecList = new List<StatKeySpec>();
            var count = 0;
            for (var i = 0; i < this.FileSpec.KeyCount; i++) {
                for (; ; ) {
                    var keySpec = new StatKeySpec(
                        (ushort)(dataBuffer.GetUInt16(16 + count * 16 + 0) - 1),
                        dataBuffer.GetUInt16(16 + count * 16 + 2),
                        (KeyFlag)dataBuffer.GetUInt16(16 + count * 16 + 4),
                        dataBuffer.GetUInt32(16 + count * 16 + 6),
                        dataBuffer[16 + count * 16 + 10],
                        dataBuffer[16 + count * 16 + 11],
                        (sbyte)dataBuffer[16 + count * 16 + 14],
                        dataBuffer[16 + count * 16 + 15]);
                    keySpecList.Add(keySpec);
                    count++;
                    if (!keySpec.IsSegmentKey) {
                        break;
                    }
                }
            }
            this.KeySpecs = keySpecList.ToArray();
        }

        public StatFileSpec FileSpec { get; private set; }
        public IEnumerable<StatKeySpec> KeySpecs { get; private set; }
    }
}
