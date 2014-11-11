using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Utilities;

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
                ((ushort)(this.Position + 1)).SetBytes(result, 0);
                this.Length.SetBytes(result, 2);
                ((ushort)this.Flag).SetBytes(result, 4);
                result[10] = this.ExtendedKeyType;
                result[11] = this.NullValue;
                result[14] = (byte)this.Number;
                result[15] = this.AcsNumber;
                return result;
            }
        }
    }
}
