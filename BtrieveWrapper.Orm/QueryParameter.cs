using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class QueryParameter<TRecord> where TRecord : Record<TRecord>
    {
        public QueryParameter(
            KeyInfo key,
            Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false) {

            this.LockMode = lockMode;
            this.StartingRecord = startingRecord;
            this.SkipStartingRecord = skipStartingRecord;
            this.Reverse = reverse;
            this.RejectCount = rejectCount;
            this.Key = key;
            this.Limit = limit;

            if (whereExpression != null) {
                var argument = whereExpression.Parameters[0];
                var parser = new ExpressionParser(whereExpression.Body, argument, isIgnoreCase);
                this.ApiFilter = parser.Filter;
                this.AdditionalFilter = parser.GetAdditionalFilter<TRecord>();
                this.Filter = whereExpression.Compile();
            }
        }

        public QueryParameter(
            Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false)
            : this(null, whereExpression, lockMode, startingRecord, skipStartingRecord, limit, reverse, rejectCount, isIgnoreCase) {

            this.Key = this.GetKey();
        }

        public KeyInfo Key { get; private set; }
        public FilterAnd ApiFilter { get; private set; }
        public Func<TRecord, bool> AdditionalFilter { get; private set; }
        public Func<TRecord, bool> Filter { get; private set; }
        public TRecord StartingRecord { get; set; }
        public LockMode LockMode { get; set; }
        public bool SkipStartingRecord { get; set; }
        public int Limit { get; set; }
        public bool Reverse { get; set; }
        public ushort RejectCount { get; set; }
        public bool UseKey {
            get {
                return this.Key != null;
            }
        }
        public KeyValue UniqueKeyValue {
            get {
                if (this.Key != null && this.ApiFilter != null && this.Key.DuplicateKeyOption == DuplicateKeyOption.Unique) {
                    try {
                        var values = new List<object>();
                        foreach (var segment in this.Key.Segments) {
                            values.Add(this.ApiFilter.Single(f => f.State == FilterOrState.Equal && f[0].Field == segment.Field)[0].Value);
                        }
                        return new KeyValue(this.Key, values.ToArray());
                    } catch { }
                }
                return null;
            }
        }

        KeyInfo GetKey() {
            if (this.ApiFilter != null) {
                var keys = Resource.GetRecordInfo(typeof(TRecord)).Keys;
                var maxKeySegmentCount = keys.Max(k => k.Segments.Count);

                IEnumerable<KeyInfo> enabledKeys = keys, remainedKeys = new KeyInfo[0];
                var enabledKeyList = new List<KeyInfo>();
                var priorityList = new List<uint>();

                var fields = this.ApiFilter
                    .SelectMany(filter => filter.GetFields().Select(field => new {
                        State = filter.State,
                        Field = field
                    }))
                    .GroupBy(f => f.Field)
                    .Select(g => new {
                        Field = g.Key,
                        Priority = GetPriority(g.Select(f=>f.State))
                    });

                for (var i = 0; i < maxKeySegmentCount; i++) {
                    uint priority = 0;
                    foreach (var key in enabledKeys) {
                        var field = fields.SingleOrDefault(f => f.Field == key.Segments[i].Field);
                        if (field != null) {
                            if (field.Priority == priority) {
                                enabledKeyList.Add(key);
                            } else if (field.Priority > priority) {
                                priority = field.Priority;
                                enabledKeyList.Clear();
                                enabledKeyList.Add(key);
                            }
                        }
                    }
                    if (enabledKeyList.Count == 0) {
                        if (remainedKeys.Count() == 0) {
                            remainedKeys = enabledKeys.Where(k => i == k.Segments.Count - 1).ToArray();
                        }
                        enabledKeys = enabledKeys.Where(k => i != k.Segments.Count - 1).ToArray();
                    } else if (enabledKeyList.Count == 1) {
                        return enabledKeyList[0];
                    } else if (enabledKeyList.Count > 1) {
                        enabledKeys = enabledKeyList.Where(k => i != k.Segments.Count - 1).ToArray();
                        remainedKeys = enabledKeyList.Where(k => i == k.Segments.Count - 1).ToArray();
                    }
                    enabledKeyList.Clear();
                    priorityList.Add(priority);
                }
                if (priorityList.Any(p => p != 0)) {
                    return remainedKeys.FirstOrDefault() ?? enabledKeys.FirstOrDefault();
                } else {
                    return null;
                }
            }
            return null;
        }

        static uint GetPriority(IEnumerable<FilterOrState> states) {
            var equalCount = states.Where(s => s == FilterOrState.Equal).Count();
            var greaterCount = states.Where(s => s == FilterOrState.Greater).Count();
            var lessCount = states.Where(s => s == FilterOrState.Less).Count();
            var fieldCount = states.Where(s => s == FilterOrState.Field).Count();
            var otherCount = states.Where(s => s == FilterOrState.Other).Count();

            if (equalCount == 1 &&
                greaterCount == 0 &&
                lessCount == 0 &&
                fieldCount == 0 &&
                otherCount == 0) {

                return 0xf0000000;
            }

            if (equalCount == 0 &&
                greaterCount == 1 &&
                lessCount == 1 &&
                fieldCount == 0 &&
                otherCount == 0) {

                return 0x00f00000;
            }

            if (equalCount == 0 &&
                ((greaterCount == 0 && lessCount == 1) ||
                (greaterCount == 1 && lessCount == 0)) &&
                fieldCount == 0 &&
                otherCount == 0) {

                return 0x000f0000;
            }


            if (equalCount == 0 &&
                greaterCount == 0 &&
                lessCount == 0 &&
                fieldCount == 1 &&
                otherCount == 0) {

                return 0x0000f000;
            }

            if (equalCount == 0 &&
                greaterCount == 0 &&
                lessCount == 0 &&
                fieldCount == 0 &&
                otherCount == 0) {

                return 0x00000000;
            }

            return 0x000000f0;
        }
    }
}
