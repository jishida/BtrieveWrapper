using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Models.Template
{
    public class TemplateParser
    {
        public static string Parse(string template, object context, string contextName = "Context") {
            if (template == null || context == null || contextName == null) {
                throw new ArgumentNullException();
            }
            var blockParser = new BlockParser(context, contextName);
            return blockParser.Parse(template);
        }
    }
}
