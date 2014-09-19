using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Binary", typeof(byte[]), IsParameterEditable = true)]
    public class ByteArrayConverter : IFieldConverter
    {
        static byte _default = 0x00;

        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            var result = new byte[length];
            Array.Copy(source, position, result, 0, length);
            return result;
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var sourceBytes = (byte[])source;
            if (sourceBytes.Length < length) {
                var defaultByte = _default;
                if (parameter != null) {
                    try {
                        defaultByte = (byte)parameter;
                    } catch {
                        throw new ArgumentException();
                    }
                }
                Array.Copy(sourceBytes, 0, destination, position, sourceBytes.Length);
                for (var i = sourceBytes.Length; i < length; i++) {
                    destination[position + i] = defaultByte;
                }
            } else {
                Array.Copy(sourceBytes, 0, destination, position, length);
            }
        }

        public ushort ConvertBack(object source, byte[] destination, ushort position, object parameter = null) {
            var sourceBytes = (byte[])source;
            int length;
            if (destination.Length < position + sourceBytes.Length) {
                length = destination.Length - position;
            } else {
                length = sourceBytes.Length;
            }
            Array.Copy(sourceBytes, 0, destination, position, length);
            return (ushort)sourceBytes.Length;
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.UnsignedBinary, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.UnsignedBinary, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {
            if (parameter != null) {
                var defaultByte = _default;
                if (parameter != null) {
                    try {
                        defaultByte = System.Convert.ToByte(parameter);
                    } catch {
                        throw new ArgumentException();
                    }
                }
                for (var i = 0; i < length; i++) {
                    buffer[position + i] = defaultByte;
                }
            }
        }

        public object GetDefaultValue() {
            return new byte[0];
        }
    }
}
