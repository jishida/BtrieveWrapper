using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public struct VariableRange
    {
        int _length;
        int _position;

        public VariableRange(int length, int position) {
            _length = length;
            _position = position;
        }

        public int Length { get { return _length; } }
        public int Position { get { return _position; } }
    }
}
