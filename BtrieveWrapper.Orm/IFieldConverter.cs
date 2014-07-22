using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public interface IFieldConverter
    {
        object Convert(byte[] source, ushort position, ushort length, object parameter);
        void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter);
        void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter);
        void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter);
        void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter);
    }
}
