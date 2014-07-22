using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FieldAttribute : Attribute
    {
        public FieldAttribute(ushort position, ushort length, KeyType keyType, Type converterType) {
            this.Position = position;
            this.Length = length;
            this.KeyType = keyType;
            this.ConverterType = converterType;
            this.Parameter = null;
            this.NullType = NullType.None;
        }

        public ushort Position { get; private set; }
        public ushort Length { get; private set; }
        public KeyType KeyType { get; private set; }
        public Type ConverterType { get; private set; }
        public object Parameter { get; set; }
        public NullType NullType { get; set; }
    }
}
