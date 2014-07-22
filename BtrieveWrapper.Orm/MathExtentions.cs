using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    static class MathExtentions
    {
        public static decimal PowerOf10(int exponent) {
            var result = 1m;
            if (exponent > 0) {
                for (var i = 0; i < exponent; i++) {
                    result = result * 10;
                }
            } else if (exponent < 0) {
                for (var i = 0; i < -exponent; i++) {
                    result = result / 10;
                }
            }
            return result;
        }
    }
}
