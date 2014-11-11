using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BtrieveWrapper.Utilities
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Bit32
    {
        [FieldOffset(0)]
        public uint UInt32;

        [FieldOffset(0)]
        public int Int32;

        [FieldOffset(0)]
        public float Float;

        [FieldOffset(0)]
        public byte Byte1;

        [FieldOffset(1)]
        public byte Byte2;

        [FieldOffset(2)]
        public byte Byte3;

        [FieldOffset(3)]
        public byte Byte4;

        public IEnumerable<byte> GetBytes(bool isLittleEndian = true) {
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                yield return this.Byte4;
                yield return this.Byte3;
                yield return this.Byte2;
                yield return this.Byte1;
            } else {
                yield return this.Byte1;
                yield return this.Byte2;
                yield return this.Byte3;
                yield return this.Byte4;
            }
        }

        public byte[] ToArray(bool isLittleEndian = true) {
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                return new byte[] { this.Byte4, this.Byte3, this.Byte2, this.Byte1 };
            } else {
                return new byte[] { this.Byte1, this.Byte2, this.Byte3, this.Byte4 };
            }
        }

        public static Bit32 FromBytes(byte[] source, int offset = 0, bool isLittleEndian = true) {
            if (source == null) throw new ArgumentNullException("source");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (source.Length < offset + 4) throw new ArgumentException("source");
            var result = default(Bit32);
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                result.Byte4 = source[offset++];
                result.Byte3 = source[offset++];
                result.Byte2 = source[offset++];
                result.Byte1 = source[offset];
            } else {
                result.Byte1 = source[offset++];
                result.Byte2 = source[offset++];
                result.Byte3 = source[offset++];
                result.Byte4 = source[offset];
            }
            return result;
        }
    }
}
