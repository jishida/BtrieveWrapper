using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("SByte", typeof(sbyte), 1)]
    public class SByteConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return (sbyte)source[position];
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            destination[position] = (byte)System.Convert.ToSByte(source);
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
