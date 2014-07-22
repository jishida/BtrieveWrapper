using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Orm;

namespace BtrieveWrapper.Orm.Models
{
    public class ConverterSetDictionary
    {
        Dictionary<int,ConverterSet> _dictionary;
        public ConverterSetDictionary() {
            _dictionary = new Dictionary<int, ConverterSet>();
        }

        public ConverterSet this[KeyType keyType, ushort length = 0, byte dec = 0] {
            get {
                return _dictionary[ConverterSetDictionary.GetKey(keyType, length, dec)];
            }
            set {
                _dictionary[ConverterSetDictionary.GetKey(keyType, length, dec)] = value;
            }
        }

        public bool ContainsKey(KeyType keyType, ushort length, byte dec) {
            return _dictionary.ContainsKey(ConverterSetDictionary.GetKey(keyType, length, dec));
        }

        static int GetKey(KeyType keyType, ushort length, byte dec) {
            var result = 0;
            result |= (int)keyType << 24;
            result |= (int)dec << 16;
            result |= length;
            return result;
        }
    }
}
