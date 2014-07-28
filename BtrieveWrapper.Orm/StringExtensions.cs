using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public static class StringExtensions
    {
        static int CompareTo(this byte[] binary1, byte[] binary2) {
            var length = binary1.Length > binary2.Length ? binary2.Length : binary1.Length;
            for (var i = 0; i < length; i++) {
                if (binary1[i] < binary2[i]) {
                    return -1;
                } else if (binary1[i] > binary2[i]) {
                    return 1;
                }
                if (i == binary1.Length - 1) {
                    if (i == binary2.Length - 1) {
                        return 0;
                    } else {
                        return -1;
                    }
                }
                if (i == binary2.Length - 1) {
                    return 1;
                }
            }
            throw new InvalidOperationException();
        }

        public static bool LessThan(this string str1, string str2) {
            var binary1 = Config.ComparedStringEncoding.GetBytes(str1);
            var binary2 = Config.ComparedStringEncoding.GetBytes(str2);
            return binary1.CompareTo(binary2) < 0;
        }
        public static bool LessThanOrEqual(this string str1, string str2) {
            var binary1 = Config.ComparedStringEncoding.GetBytes(str1);
            var binary2 = Config.ComparedStringEncoding.GetBytes(str2);
            return binary1.CompareTo(binary2) < 1;
        }
        public static bool GreaterThan(this string str1, string str2) {
            var binary1 = Config.ComparedStringEncoding.GetBytes(str1);
            var binary2 = Config.ComparedStringEncoding.GetBytes(str2);
            return binary1.CompareTo(binary2) > 0;
        }
        public static bool GreaterThanOrEqual(this string str1, string str2) {
            var binary1 = Config.ComparedStringEncoding.GetBytes(str1);
            var binary2 = Config.ComparedStringEncoding.GetBytes(str2);
            return binary1.CompareTo(binary2) > -1;
        }
    }
}
