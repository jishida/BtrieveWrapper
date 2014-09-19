using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BtrieveWrapper.Orm.Models.Template
{
    class BlockMatcher
    {
        Regex _regex;

        public BlockMatcher(BlockType type) {
            this.BlockType = type;
            _regex = new Regex(
                BlockMatcher.GetPattern(type),
                RegexOptions.Compiled | RegexOptions.Singleline);
        }

        public BlockType BlockType { get; private set; }

        static string GetPattern(BlockType type) {
            switch (type) {
                case BlockType.Tag:
                    return Configurations.TagRegexPattern;
                case BlockType.If:
                    return Configurations.IfRegexPattern;
                case BlockType.ElseIf:
                    return Configurations.ElseIfRegexPattern;
                case BlockType.Else:
                    return Configurations.ElseRegexPattern;
                case BlockType.EndIf:
                    return Configurations.EndIfRegexPattern;
                case BlockType.For:
                    return Configurations.ForRegexPattern;
                case BlockType.EndFor:
                    return Configurations.EndForRegexPattern;
                default:
                    throw new NotImplementedException();
            }
        }

        BlockMatch Match(string input) {
            var match = _regex.Match(input);
            return match.Success ? new BlockMatch(match, this.BlockType) : null;
        }

        static IEnumerable<BlockMatcher> _regexCollection = null;

        public static void Setup() {
            _regexCollection = new[]{
                new BlockMatcher(BlockType.Tag),
                new BlockMatcher(BlockType.If),
                new BlockMatcher(BlockType.ElseIf),
                new BlockMatcher(BlockType.Else),
                new BlockMatcher(BlockType.EndIf),
                new BlockMatcher(BlockType.For),
                new BlockMatcher(BlockType.EndFor)};
        }

        public static BlockMatch GetMatch(string input) {
            if (_regexCollection == null) {
                BlockMatcher.Setup();
            }
            BlockMatch result = null;
            foreach (var regex in _regexCollection) {
                result = BlockMatcher.GetMatch(result, regex.Match(input));
            }
            return result;
        }

        static BlockMatch GetMatch(BlockMatch m1, BlockMatch m2) {
            return m1 == null
                    ? m2 == null
                        ? null
                        : m2
                    : m2 == null
                        ? m1
                        : m1.Match.Index < m2.Match.Index ? m1 : m2;
        }
    }
}
