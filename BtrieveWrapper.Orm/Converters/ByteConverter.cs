﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("Byte", typeof(byte), 1)]
    public class ByteConverter : IFieldConverter
    {
        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            return source[position];
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            destination[position] = System.Convert.ToByte(source);
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
