using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public static class StringExtensions
    {
        public static bool LessThan(this string str1, string str2) {
            return str1.CompareTo(str2) < 0;
        }
        public static bool LessThanOrEqual(this string str1, string str2) {
            return str1.CompareTo(str2) < 1;
        }
        public static bool GreaterThan(this string str1, string str2) {
            return str1.CompareTo(str2) > 0;
        }
        public static bool GreaterThanOrEqual(this string str1, string str2) {
            return str1.CompareTo(str2) > -1;
        }
    }
}
