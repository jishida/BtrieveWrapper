using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class FilterOr : List<Filter>
    {
        public FilterOr() { }

        public ushort FilterCount {
            get {
                var result = (ushort)this.Count;
                if (this.FilterAnd != null) {
                    result += this.FilterAnd.FilterCount;
                }
                return result;
            }
        }

        public ushort Length {
            get {
                var result = (ushort)this.Sum(f => f.Length);
                if (this.FilterAnd != null) {
                    result += this.FilterAnd.Length;
                }
                return result;
            }
        }

        public IEnumerable<Filter> GetFilters() {
            foreach (var filter in this) {
                yield return filter;
            }
            if (this.FilterAnd != null) {
                foreach (var filter in this.FilterAnd.GetFilters()) {
                    yield return filter;
                }
            }
        }

        public FilterOr Add(FieldInfo field, FilterType type, object value, AcsIdentifier acsIdentifier = null, bool useDefaultAcs = false, bool isIgnoreCase = false) {
            return this.Add(new Filter(field, type, value, acsIdentifier, useDefaultAcs, isIgnoreCase));
        }

        public FilterAnd FilterAnd { get; set; }

        public FilterOrState State {
            get {
                if (this.Count == 1 && this.FilterAnd == null) {
                    if (this[0].ComparedField == null) {
                        switch (this[0].Type) {
                            case FilterType.Equal:
                                return FilterOrState.Equal;
                            case FilterType.GreaterThan:
                            case FilterType.GreaterThanOrEqual:
                                return FilterOrState.Greater;
                            case FilterType.LessThan:
                            case FilterType.LessThanOrEqual:
                                return FilterOrState.Less;
                        }
                    } else {
                        return FilterOrState.Field;
                    }
                }
                return FilterOrState.Other;
            }
        }

        public IEnumerable<FieldInfo> GetFields() {
            HashSet<FieldInfo> result = new HashSet<FieldInfo>();
            foreach (var filter in this.GetFilters()) {
                foreach (var field in filter.Fields) {
                    if (!result.Contains(field)) {
                        result.Add(field);
                    }
                }
            }
            return result;
        }

        public new FilterOr Add(Filter filter) {
            base.Add(filter);
            return this;
        }

        public new FilterOr AddRange(IEnumerable<Filter> filters) {
            base.AddRange(filters);
            return this;
        }

        internal ushort SetDataBuffer(byte[] dataBuffer, ushort position, bool end) {
            for (var i = 0; i < this.Count; i++) {
                var logical = i == this.Count - 1 && this.FilterAnd == null
                    ? end 
                        ? (byte)0
                        : (byte)1
                    : (byte)2;
                position = this[i].SetDataBuffer(dataBuffer, position, logical);
            }
            if (this.FilterAnd != null) {
                position = this.FilterAnd.SetDataBuffer(dataBuffer, position, end);
            }
            return position;
        }
    }
}
