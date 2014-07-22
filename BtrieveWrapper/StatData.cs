using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class StatData
    {
        internal StatData(byte[] dataBuffer) {
            this.FileSpec = new StatFileSpec(
                BitConverter.ToUInt16(dataBuffer, 0),
                BitConverter.ToUInt16(dataBuffer, 2),
                dataBuffer[4],
                dataBuffer[5],
                BitConverter.ToUInt32(dataBuffer, 6),
                (FileFlag)BitConverter.ToUInt16(dataBuffer, 10),
                dataBuffer[12],
                BitConverter.ToUInt16(dataBuffer, 14));
            var keySpecList = new List<StatKeySpec>();
            var count = 0;
            for (var i = 0; i < this.FileSpec.KeyCount; i++) {
                for (; ; ) {
                    var keySpec = new StatKeySpec(
                        (ushort)(BitConverter.ToUInt16(dataBuffer, 16 + count * 16 + 0) - 1),
                        BitConverter.ToUInt16(dataBuffer, 16 + count * 16 + 2),
                        (KeyFlag)BitConverter.ToUInt16(dataBuffer, 16 + count * 16 + 4),
                        BitConverter.ToUInt32(dataBuffer, 16 + count * 16 + 6),
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
