using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class FilterAnd : List<FilterOr>
    {
        public FilterAnd() { }

        public ushort FilterCount {
            get {
                return (ushort)this.Sum(fo => fo.FilterCount);
            }
        }

        public ushort Length {
            get {
                return (ushort)this.Sum(fo => fo.Length);
            }
        }

        public IEnumerable<Filter> GetFilters() {
            return this.SelectMany(f => f.GetFilters());
        }

        public new FilterAnd Add(FilterOr filter) {
            base.Add(filter);
            return this;
        }

        public new FilterAnd AddRange(IEnumerable<FilterOr> filters) {
            base.AddRange(filters);
            return this;
        }

        internal bool Check(RecordInfo recordInfo) {
                foreach (var filter in this.GetFilters()) {
                    if (recordInfo != filter.Field.Record) {
                        return false;
                    }
                }
            return true;
        }

        internal ushort SetDataBuffer(byte[] dataBuffer, ushort position = 8, bool end = true) {
            for (var i = 0; i < this.Count; i++) {
                position = this[i].SetDataBuffer(dataBuffer, position, end && i == this.Count - 1);
            }
            return position;
        }

    }
}
