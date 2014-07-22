using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute(sbyte keyNumber) {
            if (keyNumber < 0 || keyNumber > 118) {
                throw new ArgumentException();
            }
            this.KeyNumber = keyNumber;
            this.DuplicateKeyOption = DuplicateKeyOption.Unique;
            this.IsModifiable = false;
            this.NullKeyOption = NullKeyOption.None;
        }

        public sbyte KeyNumber { get; private set; }
        public DuplicateKeyOption DuplicateKeyOption { get; set; }
        public bool IsModifiable { get; set; }
        public NullKeyOption NullKeyOption { get; set; }
    }
}
