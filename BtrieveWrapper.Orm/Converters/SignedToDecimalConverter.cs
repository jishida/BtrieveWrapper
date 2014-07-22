using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Decimal (Signed Integer Base)", typeof(decimal), 1, 2, 4, 8,
        DefaultParameter = "0",
        ParameterList = new string[]{
            "-10", "-9", "-8", "-7", "-6", "-5", "-4", "-3", "-2", "-1",   
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" })]
    public class SignedToDecimalConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            var scale = MathExtentions.PowerOf10((parameter as int?) ?? 0);
            switch (length) {
                case 1:
                    return (sbyte)source[position] / scale;
                case 2:
                    return BitConverter.ToInt16(source, position) / scale;
                case 4:
                    return BitConverter.ToInt32(source, position) / scale;
                case 8:
                    return BitConverter.ToInt64(source, position) / scale;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter = null) {
            if (source == null) {
                throw new ArgumentNullException();
            }
            var scale = MathExtentions.PowerOf10((parameter as int?) ?? 0);
            switch (length) {
                case 1:
                    destination[position] = (byte)(System.Convert.ToDecimal(source) * scale);
                    return;
                case 2:
                    Array.Copy(BitConverter.GetBytes((short)(System.Convert.ToDecimal(source) * scale)), 0, destination, position, length);
                    return;
                case 4:
                    Array.Copy(BitConverter.GetBytes((int)(System.Convert.ToDecimal(source) * scale)), 0, destination, position, length);
                    return;
                case 8:
                    Array.Copy(BitConverter.GetBytes((long)(System.Convert.ToDecimal(source) * scale)), 0, destination, position, length);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.Integer, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.Integer, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
    }
}
