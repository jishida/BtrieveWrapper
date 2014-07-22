using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    public class UInt16Binding : Binding
    {
        public UInt16Binding() {
            this.ValidationRules.Add(new IntegerRule() { Max = 65535, Min = 0 });
            this.Converter = new UInt16Converter();
        }

        public UInt16Binding(PropertyPath path)
            : this() {
            this.Path = path;
        }
    }
}
