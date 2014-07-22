using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class StatKeySpec:KeySpec
    {
        internal StatKeySpec(
            ushort position,
            ushort length,
            KeyFlag flag,
            uint unduplicatedKeyValueCount,
            byte extendedKeyType,
            byte nullValue,
            sbyte keyNumber,
            byte acsNumber)
            : base(
                position,
                length,
                flag,
                extendedKeyType,
                nullValue,
                keyNumber,
                acsNumber) {

            this.UnduplicatedKeyValueCount = unduplicatedKeyValueCount;
        }

        public uint UnduplicatedKeyValueCount { get; private set; }
    }
}
