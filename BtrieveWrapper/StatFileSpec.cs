using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class StatFileSpec : FileSpec
    {
        internal StatFileSpec(
            ushort recordLength,
            ushort pageSize,
            byte keyCount,
            byte fileVersion,
            uint recordCount,
            FileFlag flag,
            byte unusedDuplicatedPointerCount,
            ushort unusedPage)
            : base(
            recordLength,
            pageSize,
            keyCount,
            flag) {

            this.FileVersion = fileVersion;
            this.RecordCount = recordCount;
            this.UnusedDuplicatedPointerCount = unusedDuplicatedPointerCount;
            this.UnusedPage = unusedPage;
        }

        public byte FileVersion { get; private set; }
        public uint RecordCount { get; private set; }
        public byte UnusedDuplicatedPointerCount { get; private set; }
        public ushort UnusedPage { get; private set; }
    }
}
