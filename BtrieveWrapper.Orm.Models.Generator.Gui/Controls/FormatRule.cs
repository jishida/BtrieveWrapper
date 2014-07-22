using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Controls;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    public class FormatRule : ValidationRule
    {
        public string Format { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            var target = value.ToString();
            return new ValidationResult(Regex.IsMatch(target, this.Format), null);
        }
    }
}
