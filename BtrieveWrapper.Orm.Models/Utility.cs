using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Models
{
    static class Utility
    {
        public static string Escape(this string text) {
            var builder = new StringBuilder(text);
            builder.Replace("&", "&amp;");
            builder.Replace("<", "&lt;");
            builder.Replace(">", "&gt;");
            return builder.ToString();
        }
    }
}
