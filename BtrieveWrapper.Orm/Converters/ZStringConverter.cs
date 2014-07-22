using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("ZString (Default)", 1, 255, typeof(string), IsParameterEditable = true)]
    public class ZStringConverter : IFieldConverter
    {
        public ZStringConverter() {
            this.Encoding = Encoding.Default;
        }

        public Encoding Encoding { get; protected set; }

        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return this.Encoding.GetString(source, position, length).TrimEnd('\0');
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var sourceBytes = this.Encoding.GetBytes((string)source);
            if (sourceBytes.Length < length - 1) {
                Array.Copy(sourceBytes, 0, destination, position, sourceBytes.Length);
                var zeroLength = length - sourceBytes.Length;
                for (var i = 0; i < zeroLength; i++) {
                    destination[position + sourceBytes.Length + i] = 0x00;
                }
            } else {
                Array.Copy(sourceBytes, 0, destination, position, length - 1);
                destination[position + length - 1] = 0x00;
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
            Utility.SetMaxValue(KeyType.ZString, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.ZString, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }

        public object GetDefaultValue() {
            return "";
        }
    }
}
