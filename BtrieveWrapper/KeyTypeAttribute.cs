using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class KeyTypeAttribute : Attribute
    {
        public KeyTypeAttribute(string name, ushort defaultLength, params ushort[] lengthList) {
            this.Name = name;
            this.DefaultLength = defaultLength;
            this.LengthList = lengthList;
        }

        public string Name { get; private set; }
        public ushort DefaultLength { get; private set; }
        public IEnumerable<ushort> LengthList { get; private set; }

        public bool ValidateLength(ushort length) {
            if (this.LengthList.Count() == 0) {
                return true;
            } else {
                return this.LengthList.Any(l => l == length);
            }
        }
    }
}
