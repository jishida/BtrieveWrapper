using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class KeyCollection : IEnumerable<KeyInfo>
    {
        KeyInfo[] _keys;
        int _count = -1;

        internal KeyCollection(IEnumerable<KeyInfo> keys) {
            _keys = keys.ToArray();
        }

        public KeyCollection(Type recordType) {
            _keys = Resource.GetRecordInfo(recordType).Keys.ToArray();
        }

        public KeyInfo this[sbyte keyNumber] {
            get {
                return _keys.SingleOrDefault(k => k.KeyNumber == keyNumber);
            }
        }

        public int Count {
            get { return _count == -1 ? _count = _keys.Count() : _count; }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)_keys).GetEnumerator();
        }

        public IEnumerator<KeyInfo> GetEnumerator() {
            return ((IEnumerable<KeyInfo>)_keys).GetEnumerator();
        }
    }

    public class KeyCollection<TRecord> : KeyCollection where TRecord : Record<TRecord>
    {
        public KeyCollection() : base(typeof(TRecord)) { }
    }
}
