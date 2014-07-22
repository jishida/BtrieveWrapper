using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Binary String", typeof(string))]
    public class BinaryStringConverter : IFieldConverter
    {
        public BinaryStringConverter() { }

        static byte GetByte(char digit) {
            var digitByte = (byte)digit;
            if (digitByte >= 0x30 && digitByte <= 0x39) {
                return (byte)(digitByte - 0x30);
            }
            if (digitByte >= 0x41 && digitByte <= 0x46) {
                return (byte)(digitByte - 0x3B);
            }
            if (digitByte >= 0x61 && digitByte <= 0x66) {
                return (byte)(digitByte - 0x5B);
            }
            throw new ArgumentException();
        }

        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            var result = new StringBuilder();
            for (var i = 0; i < length;i++ ) {
                result.Append(source[position + i].ToString("X2"));
            }
            return result.ToString();
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var sourceString = ((string)source).ToUpper();
            if (sourceString.Length != length * 2) {
                throw new ArgumentException();
            }
            for (var i = 0; i <length; i++) {
                destination[position + i] = (byte)((GetByte(sourceString[i * 2]) << 4) |( GetByte(sourceString[i * 2 + 1])));
            }
        }

        public ushort ConvertBack(object source, byte[] destination, ushort position, object parameter) {
            var sourceString = ((string)source).ToUpper();
            var length = (ushort)(sourceString.Length / 2);
            if (sourceString.Length != length * 2) {
                throw new ArgumentException();
            }
            for (var i = 0; i < length; i++) {
                destination[position + i] = (byte)((GetByte(sourceString[i * 2]) << 4) | (GetByte(sourceString[i * 2 + 1])));
            }
            return length;
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.String, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.String, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }

        public object GetDefaultValue(){
            return "";
        }
    }
}
