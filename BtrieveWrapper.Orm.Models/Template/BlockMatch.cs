using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BtrieveWrapper.Orm.Models.Template
{
    class BlockMatch
    {
        public BlockMatch(Match match, BlockType type){
            this.Match = match;
            this.Type = type;
        }

        public Match Match { get; private set; }
        public BlockType Type { get; private set; }
    }
}
