using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    public class SByteBinding : Binding
    {
        public SByteBinding() {
            this.ValidationRules.Add(new IntegerRule() { Max = 127, Min = -128 });
            this.Converter = new SByteConverter();
        }

        public SByteBinding(PropertyPath path)
            : this() {
            this.Path = path;
        }
    }
}
