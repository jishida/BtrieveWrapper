using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    public class ByteBinding : Binding
    {
        public ByteBinding() {
            this.ValidationRules.Add(new IntegerRule() { Max = 255, Min = 0 });
            this.Converter = new ByteConverter();
        }

        public ByteBinding(PropertyPath path)
            : this() {
            this.Path = path;
        }
    }
}
