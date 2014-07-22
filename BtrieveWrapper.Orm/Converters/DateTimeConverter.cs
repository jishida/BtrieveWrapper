using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("DateTime", typeof(DateTime), 8)]
    public class DateTimeConverter : IFieldConverter
    {
        static readonly DateTime _standard = new DateTime(1900, 1, 1);
        static readonly DateTime _minimum = new DateTime(1753, 1, 1);
        static readonly DateTime _maximum = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            var daySpan = TimeSpan.FromDays(BitConverter.ToInt32(source, position));
            var millisecondSpan=TimeSpan.FromMilliseconds(BitConverter.ToInt32(source, position+4));
            return _standard.Add(daySpan).Add(millisecondSpan);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var date = (DateTime)source;
            if (date < _minimum) {
                throw new ArgumentOutOfRangeException();
            }
            var days = (date.Date - _standard).Days;
            var milliseconds = date.Hour * 60 * 60 * 1000 + date.Minute * 60 * 1000 + date.Second * 1000 + date.Millisecond;
            Array.Copy(BitConverter.GetBytes(days), 0, destination, position, 4);
            Array.Copy(BitConverter.GetBytes(milliseconds), 0, destination, position + 4, 4);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            this.ConvertBack(_maximum, buffer, position, length, null);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            this.ConvertBack(_minimum, buffer, position, length, null);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
    }
}
