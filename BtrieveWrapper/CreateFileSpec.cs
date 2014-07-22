using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                Array.Copy(BitConverter.GetBytes(this.RecordLength), 0, result, 0, 2);
                Array.Copy(BitConverter.GetBytes(this.PageSize), 0, result, 2, 2);
                Array.Copy(BitConverter.GetBytes(this.KeyCount), 0, result, 4, 2);
                Array.Copy(BitConverter.GetBytes((ushort)this.Flag), 0, result, 10, 2);
                result[13] = this.DuplicatedPointerCount;
                Array.Copy(BitConverter.GetBytes(Allocation), 0, result, 14, 2);
                return result;
            }
        }
    }
}
