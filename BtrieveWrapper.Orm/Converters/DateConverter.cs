using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Date", typeof(DateTime), 4)]
    public class DateConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter = null) {
            return new DateTime(BitConverter.ToInt16(source, position + 2), source[position + 1], source[position]);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter = null) {
            var datetime = (DateTime)source;
            destination[position] = (byte)datetime.Day;
            destination[position+1] = (byte)datetime.Month;
            Array.Copy(BitConverter.GetBytes((short)datetime.Year), 0, destination, position + 2, 2);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.Date, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.Date, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.Date, buffer, position, length, parameter);
        }
    }
}
