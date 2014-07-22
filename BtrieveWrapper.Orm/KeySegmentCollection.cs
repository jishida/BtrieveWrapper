using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class KeySegmentCollection : IEnumerable<KeySegmentInfo>
    {
        KeySegmentInfo[] _segments;
        int _count=-1;
        internal KeySegmentCollection(IEnumerable<KeySegmentInfo> segments) {
            _segments = segments.ToArray();
        }

        public KeySegmentInfo this[int index] {
            get { return _segments[index]; }
        }

        public int Count { get { return _count == -1 ? _count = _segments.Count() : _count; } }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)_segments).GetEnumerator();
        }

        public IEnumerator<KeySegmentInfo> GetEnumerator() {
            return ((IEnumerable<KeySegmentInfo>)_segments).GetEnumerator();
        }
    }
}
