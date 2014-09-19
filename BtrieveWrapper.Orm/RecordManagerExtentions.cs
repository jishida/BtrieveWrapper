using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public static class RecordManagerExtentions
    {
        public static TRecord GetByKey<TRecord, TKeyCollection>(
            this RecordReader<TRecord, TKeyCollection> reader,
            Func<TKeyCollection, KeyInfo> keySelector,
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {

            return reader.GetByKey(
                keySelector == null ? null : keySelector(reader.Keys), 
                whereExpression, 
                lockMode);
        }

        public static TRecord GetByKeyValue<TRecord, TKeyCollection>(
            this RecordReader<TRecord, TKeyCollection> reader, 
            Func<TKeyCollection, KeyValue> keyValueSelector,
            LockMode lockMode = LockMode.None)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {

            if (keyValueSelector == null) {
                throw new ArgumentNullException();
            }
            return reader.GetByKeyValue(
                keyValueSelector(reader.Keys), 
                lockMode);
        }

        public static IEnumerable<TRecord> QueryByKey<TRecord, TKeyCollection>(
            this RecordReader<TRecord, TKeyCollection> reader,
            Func<TKeyCollection, KeyInfo> keySelector,
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {

            return reader.QueryByKey(
                keySelector == null ? null : keySelector(reader.Keys),
                whereExpression,
                lockMode,
                startingRecord,
                skipStartingRecord,
                limit,
                reverse,
                rejectCount,
                isIgnoreCase);
        }

        public static TRecord GetByKeyAndManage<TRecord, TKeyCollection>(
            this RecordManager<TRecord, TKeyCollection> manager,
            Func<TKeyCollection, KeyInfo> keySelector,
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {

            return manager.GetByKeyAndManage(
                keySelector == null ? null : keySelector(manager.Keys), 
                whereExpression, 
                lockMode);
        }

        public static TRecord GetByKeyValueAndManage<TRecord, TKeyCollection>(
            this RecordManager<TRecord, TKeyCollection> manager, Func<TKeyCollection, KeyValue> keyValueSelector, 
            LockMode lockMode = LockMode.None)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {

            if (keyValueSelector == null) {
                throw new ArgumentNullException();
            }
            return manager.GetByKeyValueAndManage(
                keyValueSelector(manager.Keys), 
                lockMode);
        }

        public static IEnumerable<TRecord> QueryByKeyAndManage<TRecord, TKeyCollection>(
            this RecordManager<TRecord, TKeyCollection> manager,
            Func<TKeyCollection, KeyInfo> keySelector,
            System.Linq.Expressions.Expression<Func<TRecord, bool>> whereExpression = null,
            LockMode lockMode = LockMode.None,
            TRecord startingRecord = null,
            bool skipStartingRecord = false,
            int limit = 0,
            bool reverse = false,
            ushort rejectCount = 0,
            bool isIgnoreCase = false)
            where TRecord : Record<TRecord>
            where TKeyCollection : KeyCollection<TRecord>, new() {

            return manager.QueryByKeyAndManage(
                keySelector == null ? null : keySelector(manager.Keys),
                whereExpression,
                lockMode,
                startingRecord,
                skipStartingRecord,
                limit,
                reverse,
                rejectCount,
                isIgnoreCase);
        }
    }
}
