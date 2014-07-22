using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Float", typeof(float), 4)]
    public class FloatConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter = null) {
            return BitConverter.ToSingle(source, position);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter = null) {
            if (source == null) {
                throw new ArgumentNullException();
            }
            Array.Copy(BitConverter.GetBytes((float)source), 0, destination, position, length);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMaxValue(KeyType.Float, buffer, position, length, parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            Utility.SetMinValue(KeyType.Float, buffer, position, length, parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {
            
        }
    }
}
