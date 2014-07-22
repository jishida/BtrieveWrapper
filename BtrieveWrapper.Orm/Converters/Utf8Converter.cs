using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Converters
{
    [FieldConverter("String (UTF8)", typeof(string), IsParameterEditable = true)]
    public class Utf8Converter : StringConverter
    {
        public Utf8Converter() {
            this.Encoding = Encoding.UTF8;
        }
    }
}
