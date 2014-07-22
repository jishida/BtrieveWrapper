using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("UInt64", typeof(ulong), 8)]
    public class UInt64Converter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return BitConverter.ToUInt64(source, position);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            Array.Copy(BitConverter.GetBytes(System.Convert.ToUInt64(source)), 0, destination, position, length);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.UnsignedBinary, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.UnsignedBinary, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
    }
}
