using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Boolean", typeof(bool))]
    public class BooleanConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return !source.Where((s, i) => i >= position && i < position + length).All(s => s == 0x00);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            for (var i = 0; i < length; i++) {
                destination[position + i] = 0x00;
            }
            if ((bool)source) {
                destination[position] = 0x01;
            }
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
