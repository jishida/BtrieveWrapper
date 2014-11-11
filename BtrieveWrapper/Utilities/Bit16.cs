using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BtrieveWrapper.Utilities
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Bit16
    {
        [FieldOffset(0)]
        public ushort UInt16;

        [FieldOffset(0)]
        public short Int16;

        [FieldOffset(0)]
        public byte Byte1;

        [FieldOffset(1)]
        public byte Byte2;

        public IEnumerable<byte> GetBytes(bool isLittleEndian = true) {
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                yield return this.Byte2;
                yield return this.Byte1;
            } else {
                yield return this.Byte1;
                yield return this.Byte2;
            }
        }

        public byte[] ToArray(bool isLittleEndian = true) {
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                return new byte[] { this.Byte2, this.Byte1 };
            } else {
                return new byte[] { this.Byte1, this.Byte2 };
            }
        }

        public static Bit16 FromBytes(byte[] source, int offset = 0, bool isLittleEndian = true) {
            if (source == null) throw new ArgumentNullException("source");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (source.Length < offset + 2) throw new ArgumentException("source");
            var result = default(Bit16);
            if (isLittleEndian ^ BitConverter.IsLittleEndian) {
                result.Byte2 = source[offset++];
                result.Byte1 = source[offset];
            } else {
                result.Byte1 = source[offset++];
                result.Byte2 = source[offset];
            }
            return result;
        }
    }
}
