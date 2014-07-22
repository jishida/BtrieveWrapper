using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Models.Template
{
    static class Utilities
    {
        public static string UnescapeValue(this string target) {
            return Configurations.UnescapeValueFunc(target);
        }

        public static string UnescapeTemplate(this string target) {
            return Configurations.UnescapeTemplateFunc(target);
        }

        public static object GetDefault(this Type type) {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
