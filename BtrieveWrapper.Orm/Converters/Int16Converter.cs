using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Int16", typeof(short), 2)]
    public class Int16Converter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return BitConverter.ToInt16(source, position);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            Array.Copy(BitConverter.GetBytes(System.Convert.ToInt16(source)), 0, destination, position, length);
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
