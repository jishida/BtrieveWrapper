using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class CreateKeySpec:KeySpec
    {
        public CreateKeySpec(
            ushort position,
            ushort length,
            KeyFlag flag,
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
                acsNumber) { }

        public byte[] Binary {
            get {
                var result = new byte[16];
                Array.Copy(BitConverter.GetBytes((ushort)(this.Position + 1)), 0, result, 0, 2);
                Array.Copy(BitConverter.GetBytes(this.Length), 0, result, 2, 2);
                Array.Copy(BitConverter.GetBytes((ushort)this.Flag), 0, result, 4, 2);
                result[10] = this.ExtendedKeyType;
                result[11] = this.NullValue;
                result[14] = (byte)this.Number;
                result[15] = this.AcsNumber;
                return result;
            }
        }
    }
}
