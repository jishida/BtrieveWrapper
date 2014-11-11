using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Utilities
{
    public static class BitExtensions
    {
        public static byte[] GetBytes(this ushort source, bool isLittleEndian = true) {
            var bits = default(Bit16);
            bits.UInt16 = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this short source, bool isLittleEndian = true) {
            var bits = default(Bit16);
            bits.Int16 = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this uint source, bool isLittleEndian = true) {
            var bits = default(Bit32);
            bits.UInt32 = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this int source, bool isLittleEndian = true) {
            var bits = default(Bit32);
            bits.Int32 = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this ulong source, bool isLittleEndian = true) {
            var bits = default(Bit64);
            bits.UInt64 = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this long source, bool isLittleEndian = true) {
            var bits = default(Bit64);
            bits.Int64 = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this float source, bool isLittleEndian = true) {
            var bits = default(Bit32);
            bits.Float = source;
            return bits.ToArray(isLittleEndian);
        }

        public static byte[] GetBytes(this double source, bool isLittleEndian = true) {
            var bits = default(Bit64);
            bits.Double = source;
            return bits.ToArray(isLittleEndian);
        }

        public static void SetBytes(this ushort source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 2) throw new ArithmeticException("destination");
            var bits = default(Bit16);
            bits.UInt16 = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this short source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 2) throw new ArithmeticException("destination");
            var bits = default(Bit16);
            bits.Int16 = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this uint source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 4) throw new ArithmeticException("destination");
            var bits = default(Bit32);
            bits.UInt32 = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this int source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 4) throw new ArithmeticException("destination");
            var bits = default(Bit32);
            bits.Int32 = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this ulong source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 8) throw new ArithmeticException("destination");
            var bits = default(Bit64);
            bits.UInt64 = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this long source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 8) throw new ArithmeticException("destination");
            var bits = default(Bit64);
            bits.Int64 = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this float source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 4) throw new ArithmeticException("destination");
            var bits = default(Bit32);
            bits.Float = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static void SetBytes(this Double source, byte[] destination, int offset = 0, bool isLittleEndian = true) {
            if (destination == null) throw new ArgumentNullException("destination");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (destination.Length < offset + 4) throw new ArithmeticException("destination");
            var bits = default(Bit64);
            bits.Double = source;
            foreach (var b in bits.GetBytes(isLittleEndian)) {
                destination[offset++] = b;
            }
        }

        public static ushort GetUInt16(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit16.FromBytes(source, offset, isLittleEndian).UInt16;
        }

        public static short GetInt16(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit16.FromBytes(source, offset, isLittleEndian).Int16;
        }

        public static uint GetUInt32(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit32.FromBytes(source, offset, isLittleEndian).UInt32;
        }

        public static int GetInt32(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit32.FromBytes(source, offset, isLittleEndian).Int32;
        }

        public static ulong GetUInt64(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit64.FromBytes(source, offset, isLittleEndian).UInt64;
        }

        public static long GetInt64(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit64.FromBytes(source, offset, isLittleEndian).Int64;
        }

        public static float GetFloat(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit32.FromBytes(source, offset, isLittleEndian).Float;
        }

        public static double GetDouble(this byte[] source, int offset = 0, bool isLittleEndian = true) {
            return Bit64.FromBytes(source, offset, isLittleEndian).Double;
        }
    }
}
