using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FieldConverterAttribute : Attribute
    {
        public FieldConverterAttribute(string typeName, Type convertType, params ushort[] lengthList) {
            if (convertType == null) {
                throw new ArgumentNullException();
            }
            this.TypeName = typeName;
            this.ConvertType = convertType;
            this.LengthList = lengthList == null || lengthList.Where(l => l != 0).Count() == 0
                ? null
                : lengthList.Where(l => l != 0).OrderBy(l => l);
            this.IsParameterEditable = false;
            this.DefaultParameter = null;
            this.ParameterList = null;
            this.IsNullable = false;
            this.IsFilterable = true;
        }

        public FieldConverterAttribute(string typeName, ushort startLength, ushort endLength, Type convertType)
            : this(typeName, convertType, GetLengthList(startLength, endLength)) { }

        public string TypeName { get; private set; }
        public Type ConvertType { get; private set; }
        public IEnumerable<ushort> LengthList { get; private set; }
        public bool IsParameterEditable { get; set; }
        public string DefaultParameter { get; set; }
        public string[] ParameterList { get; set; }
        public bool IsNullable { get; set; }
        public bool IsFilterable { get; set; }

        static ushort[] GetLengthList(ushort start, ushort end) {
            var count = end - start + 1;
            var result = new ushort[count];
            for (var i = 0; i < count; i++) {
                result[i] = (ushort)(start + i);
            }
            return result;
        }

        public bool ValidateLength(ushort length) {
            if (this.LengthList == null) {
                return true;
            } else {
                return this.LengthList.Any(l => l == length);
            }
        }
    }
}
