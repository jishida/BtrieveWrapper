using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Orm;

namespace BtrieveWrapper.Orm.Models
{
    public class ConverterSet
    {
        public ConverterSet(Type converterType, string parameter = null) {
            if (converterType == null) {
                throw new ArgumentNullException();
            }
            if (!converterType.GetInterfaces().Any(i => i == typeof(IFieldConverter))) {
                throw new ArgumentException();
            }
            this.ConverterType = converterType;
            this.Parameter = parameter;
        }

        public Type ConverterType { get; private set; }
        public string Parameter { get; private set; }
    }
}
