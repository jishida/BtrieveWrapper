using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BtrieveWrapper.Utilities
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Bit64
    {
        [FieldOffset(0)]
        public ulong UInt64;

        [FieldOffset(0)]
        public long Int64;

        [FieldOffset(0)]
        public double Double;

        [FieldOffset(0)]
        public byte Byte1;

        [FieldOffset(1)]
        public byte Byte2;

        [FieldOffset(2)]
        public byte Byte3;

        [FieldOffset(3)]
        public byte Byte4;

        [FieldOffset(4)]
        public byte Byte5;

        [FieldOffset(5)]
        public byte Byte6;

        [FieldOffset(6)]
        public byte Byte7;

        [FieldOffset(7)]
        public byte Byte8;

        public IEnumerable<byte> GetBytes(bool isLittleEndian = true) {
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                yield return this.Byte8;
                yield return this.Byte7;
                yield return this.Byte6;
                yield return this.Byte5;
                yield return this.Byte4;
                yield return this.Byte3;
                yield return this.Byte2;
                yield return this.Byte1;
            } else {
                yield return this.Byte1;
                yield return this.Byte2;
                yield return this.Byte3;
                yield return this.Byte4;
                yield return this.Byte5;
                yield return this.Byte6;
                yield return this.Byte7;
                yield return this.Byte8;
            }
        }

        public byte[] ToArray(bool isLittleEndian = true) {
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                return new byte[] { this.Byte8, this.Byte7, this.Byte6, this.Byte5, this.Byte4, this.Byte3, this.Byte2, this.Byte1 };
            } else {
                return new byte[] { this.Byte1, this.Byte2, this.Byte3, this.Byte4, this.Byte5, this.Byte6, this.Byte7, this.Byte8 };
            }
        }

        public static Bit64 FromBytes(byte[] source, int offset = 0, bool isLittleEndian = true) {
            if (source == null) throw new ArgumentNullException("source");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (source.Length < offset + 8) throw new ArgumentException("source");
            var result = default(Bit64);
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                result.Byte8 = source[offset++];
                result.Byte7 = source[offset++];
                result.Byte6 = source[offset++];
                result.Byte5 = source[offset++];
                result.Byte4 = source[offset++];
                result.Byte3 = source[offset++];
                result.Byte2 = source[offset++];
                result.Byte1 = source[offset];
            } else {
                result.Byte1 = source[offset++];
                result.Byte2 = source[offset++];
                result.Byte3 = source[offset++];
                result.Byte4 = source[offset++];
                result.Byte5 = source[offset++];
                result.Byte6 = source[offset++];
                result.Byte7 = source[offset++];
                result.Byte8 = source[offset];
            }
            return result;
        }
    }
}
