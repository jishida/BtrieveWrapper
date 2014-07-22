using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class KeySegmentAttribute : Attribute
    {
        public KeySegmentAttribute(sbyte keyNumber, ushort index) {
            if (keyNumber < 0 || keyNumber > 118) {
                throw new ArgumentException();
            }
            this.KeyNumber = keyNumber;
            this.Index = index;
            this.NullValue = 0x00;
            this.IsDescending = false;
            this.IsIgnoreCase = false;
        }

        public sbyte KeyNumber { get; private set; }
        public ushort Index { get; private set; }
        public byte NullValue { get; set; }
        public bool IsDescending { get; set; }
        public bool IsIgnoreCase { get; set; }
    }
}
