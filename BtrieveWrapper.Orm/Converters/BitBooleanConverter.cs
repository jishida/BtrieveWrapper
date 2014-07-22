using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Bit", typeof(bool), 1,
        DefaultParameter = "Bit.Bit1",
        ParameterList = new[]{
            "Bit.Bit1",
            "Bit.Bit2",
            "Bit.Bit3",
            "Bit.Bit4",
            "Bit.Bit5",
            "Bit.Bit6",
            "Bit.Bit7",
            "Bit.Bit8" },
        IsFilterable = false)]
    public class BitBooleanConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            var bit = (Bit)parameter;
            return (source[position] & (byte)bit) == (byte)bit;
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            var bit = (Bit)parameter;
            destination[position] = System.Convert.ToBoolean(source)
                ? (byte)(destination[position] | (byte)bit)
                : (byte)(destination[position] & ~(byte)bit);
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
