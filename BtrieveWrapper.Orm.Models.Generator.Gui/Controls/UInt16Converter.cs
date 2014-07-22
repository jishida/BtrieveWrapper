using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    [ValueConversion(typeof(ushort), typeof(String))]
    public class UInt16Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return ushort.Parse(value.ToString());
        }
    }
}
