using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("VariableRange", typeof(VariableRange), 8, IsFilterable = false)]
    public class VariableRangeConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return new VariableRange(BitConverter.ToInt32(source, position), BitConverter.ToInt32(source, position + 4));
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var range = (VariableRange)source;
            Array.Copy(BitConverter.GetBytes(range.Length), 0, destination, position, 4);
            Array.Copy(BitConverter.GetBytes(range.Position), 0, destination, position + 4, 4);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
        
        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {

        }
    }
}
