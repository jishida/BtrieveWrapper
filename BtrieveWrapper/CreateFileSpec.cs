using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Utilities;

namespace BtrieveWrapper
{
    public class CreateFileSpec : FileSpec
    {
        public const ushort DefaultPageSize = 4096;
        public const FileFlag DefaultFlag = FileFlag.None;

        public CreateFileSpec(
            ushort recordLength,
            ushort pageSize = DefaultPageSize,
            FileFlag flag = DefaultFlag,
            byte duplicatedPointerCount = 0,
            ushort allocation = 0)
            : base(
            recordLength,
            pageSize,
            0,
            flag) {

            if (recordLength == 0) {
                throw new ArgumentException();
            }

            this.DuplicatedPointerCount = duplicatedPointerCount;
            this.Allocation = allocation;
        }
        public byte DuplicatedPointerCount { get; private set; }
        public ushort Allocation { get; private set; }
        public byte[] Binary {
            get {
                var result = new byte[16];
                this.RecordLength.SetBytes(result, 0);
                this.PageSize.SetBytes(result, 2);
                ((ushort)this.KeyCount).SetBytes(result, 4);
                ((ushort)this.Flag).SetBytes(result, 10);
                result[13] = this.DuplicatedPointerCount;
                this.Allocation.SetBytes(result, 14);
                return result;
            }
        }
    }
}
