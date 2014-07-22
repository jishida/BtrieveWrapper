using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Models.Template
{
    class Configurations
    {
        public static string RootMemberRegexPattern { get;private set; }
        public static string MemberRegexPattern { get;private set; }
        public static string TagRegexPattern { get; set; }
        public static string IfRegexPattern { get; set; }
        public static string ElseRegexPattern { get; set; }
        public static string ElseIfRegexPattern { get; set; }
        public static string EndIfRegexPattern { get; set; }
        public static string ForRegexPattern { get; set; }
        public static string EndForRegexPattern { get; set; }
        public static Func<string, string> UnescapeValueFunc { get; set; }
        public static Func<string, string> UnescapeTemplateFunc { get; set; }

        static Configurations() {
            Configurations.UnescapeValueFunc = target => {
                var result = new StringBuilder();
                var escape = false;
                foreach (var c in target) {
                    if (escape) {
                        switch (c) {
                            case '\\':
                            case '"':
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        result.Append(c);
                        escape = false;
                    } else {
                        if (c == '\\') {
                            escape = true;
                        } else {
                            result.Append(c);
                        }
                    }
                }
                return result.ToString();
            };
            Configurations.UnescapeTemplateFunc = target => {
                var result = new StringBuilder(target);
                result.Replace("&et;", "%}");
                result.Replace("&bt;", "{%");
                result.Replace("&amp;", "&");
                return result.ToString();
            };

            Configurations.RootMemberRegexPattern = @"([a-zA-Z_][0-9a-zA-Z_]*?)";
            Configurations.MemberRegexPattern = @"({root}(\.{root})*?)"
                .Replace("{root}", Configurations.RootMemberRegexPattern);
            Configurations.TagRegexPattern = @"{%\s*(?<member>{member})\s*%}"
                .Replace("{member}", Configurations.MemberRegexPattern);
            Configurations.IfRegexPattern = @"{%\s*if\s+(?<member>{member})\s*(=\s*""(?<value>([^""\\]|\\""|\\\\)*?)""\s*)?%}"
                .Replace("{member}", Configurations.MemberRegexPattern);
            Configurations.ElseIfRegexPattern = @"{%\s*%elseif\s+(?<member>{member})\s*(=\s*""(?<value>([^""]|\""))""\s*)?%}"
                .Replace("{member}", Configurations.MemberRegexPattern);
            Configurations.ElseRegexPattern = @"{%\s*else\s*%}";
            Configurations.EndIfRegexPattern = @"{%\s*endif\s*%}";
            Configurations.ForRegexPattern = @"{%\s*for\s+(?<item>{root})\s+in\s+(?<list>{member})\s*%}"
                .Replace("{root}", Configurations.RootMemberRegexPattern)
                .Replace("{member}", Configurations.MemberRegexPattern); 
            Configurations.EndForRegexPattern = @"{%\s*endfor\s*%}";
        }
    }
}
