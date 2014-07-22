using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Decimal", 1, 14, typeof(decimal),
        DefaultParameter = "0",
        ParameterList = new string[]{
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" })]
    public class DecimalConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            var scale = MathExtentions.PowerOf10((parameter as int?) ?? 0);
            var result = 0m;
            for (var i = 0; i < length; i++) {
                if (i == length - 1) {
                    result += 1m * (source[position + i] >> 4);
                    if ((source[position + i] & 0x0f) == 0x0d) {
                        result = -result;
                    }
                } else {
                    result += MathExtentions.PowerOf10((length - i) * 2 - 2) * (source[position + i] >> 4);
                    result += MathExtentions.PowerOf10((length - i) * 2 - 3) * (source[position + i] & 0x0f);
                }
            }
            return result / scale;
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter = null) {
            if (source == null) {
                throw new ArgumentNullException();
            }
            var signible = (byte)0x0f;
            var scale = MathExtentions.PowerOf10((parameter as int?) ?? 0);
            var value = Math.Truncate(System.Convert.ToDecimal(source) * scale);
            if (value < 0) {
                value = -value;
                signible = (byte)0x0d;
            }
            var valueString = value.ToString();
            if (valueString.Length > length * 2 - 1) {
                throw new ArgumentException();
            }
            var zerofillCount = length * 2 - 1 - valueString.Length;
            if (zerofillCount != 0) {
                valueString = new string('0', zerofillCount) + valueString;
            }
            for (var i = 0; i < length; i++) {
                var digit = 0;
                digit |= byte.Parse(valueString.Substring(i * 2, 1)) << 4;
                if (i == length - 1) {
                    digit |= signible;
                } else {
                    digit |= byte.Parse(valueString.Substring(i * 2, 1));
                }
                destination[position + i] = (byte)digit;
            }
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.Decimal, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.Decimal, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
    }
}
