using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter(null, typeof(bool), 1)]
    public class NullFlagConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return source[position] != 0;
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            destination[position] = (byte)((bool)source ? 0x01 : 0x00);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            buffer[position] = 0x01;
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            buffer[position] = 0x00;
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {
            buffer[position] = 0x01;
        }
    }
}
