﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    [ValueConversion(typeof(byte), typeof(String))]
    public class ByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return byte.Parse(value.ToString());
        }
    }
}
