using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    public class FormatBinding : Binding
    {
        FormatRule _rule;

        public FormatBinding() {
            _rule=new FormatRule();
            this.ValidationRules.Add(_rule);
        }

        public FormatBinding(PropertyPath path)
            : this() {
            this.Path = path;
        }

        public FormatBinding(PropertyPath path, string format)
            : this(path) {
            this.Format = format;
        }

        public string Format { get; set; }
    }
}
