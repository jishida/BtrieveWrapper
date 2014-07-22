using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Timestamp", typeof(DateTime), 8)]
    public class TimestampConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return new DateTime(BitConverter.ToInt64(source, position));
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            Array.Copy(BitConverter.GetBytes(((DateTime)source).Ticks), 0, destination, position, length);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.Timestamp, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.Timestamp, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
    }
}
