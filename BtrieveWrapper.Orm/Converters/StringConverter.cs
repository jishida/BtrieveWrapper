using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("String (Default)", typeof(string), IsParameterEditable = true)]
    public class StringConverter : IFieldConverter
    {
        public StringConverter() {
            this.Encoding = Encoding.Default;
        }

        public Encoding Encoding { get; protected set; }

        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            char defaultChar = '\0';
            if (parameter != null) {
                try {
                    defaultChar = (Char)System.Convert.ToByte(parameter);
                } catch {
                    throw new ArgumentException();
                }
            }
            return this.Encoding.GetString(source, position, length).TrimEnd(defaultChar);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var sourceBytes = this.Encoding.GetBytes((string)source);
            if (sourceBytes.Length < length) {
                var defaultByte = (byte)0x00;
                if (parameter != null) {
                    try {
                        defaultByte = System.Convert.ToByte(parameter);
                    } catch {
                        throw new ArgumentException();
                    }
                }
                Array.Copy(sourceBytes, 0, destination, position, sourceBytes.Length);
                var defaultBytes = new byte[length - sourceBytes.Length];
                for (var i = 0; i < defaultBytes.Length; i++) {
                    defaultBytes[i] = defaultByte;
                }
                Array.Copy(defaultBytes, 0, destination, position + sourceBytes.Length, defaultBytes.Length);
            } else {
                Array.Copy(sourceBytes, 0, destination, position, length);
            }
        }

        public ushort ConvertBack(object source, byte[] destination, ushort position, object parameter) {
            var sourceBytes = this.Encoding.GetBytes((string)source);
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
            Utility.SetMaxValue(KeyType.String, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.String, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {
            var defaultByte = (byte)0x00;
            if (parameter != null) {
                try {
                    defaultByte = System.Convert.ToByte(parameter);
                } catch {
                    throw new ArgumentException();
                }
            }
            for (var i = 0; i < length; i++) {
                buffer[i] = defaultByte;
            }
        }
    }
}
