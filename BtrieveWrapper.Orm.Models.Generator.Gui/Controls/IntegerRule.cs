using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Controls;

namespace BtrieveWrapper.Orm.Models.Generator.Gui.Controls
{
    public class IntegerRule : ValidationRule
    {
        public long Max { get; set; }
        public long Min { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            try {
                var target = Convert.ToInt64(value);
                return new ValidationResult(target <= this.Max && target >= this.Min, null);
            } catch {
                return new ValidationResult(false, null);
            }
        }
    }
}
