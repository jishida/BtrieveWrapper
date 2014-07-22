using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BtrieveWrapper.Orm
{
    static class Resource
    {
        static Queue<byte[]> BufferQueue = new Queue<byte[]>();
        internal static object[] EmptySegmentValues = new object[0];
        static Dictionary<Type, FieldConverterAttribute> _fieldConverterAttributeDictionary = new Dictionary<Type, FieldConverterAttribute>();
        static Dictionary<Type, IFieldConverter> _fieldConverterDictionary = new Dictionary<Type, IFieldConverter>();
        static Dictionary<Type, RecordInfo> _recordDictionary = new Dictionary<Type, RecordInfo>();
        static Dictionary<KeyType, KeyTypeAttribute> _keyTypeDictionary = new Dictionary<KeyType, KeyTypeAttribute>();
        internal static Dictionary<MemberInfo, FieldInfo> _fieldMemberInfoDictionary = new Dictionary<MemberInfo, FieldInfo>();
        internal static Dictionary<string, FieldInfo> _fieldNameDictionary = new Dictionary<string, FieldInfo>();
        static Dictionary<Type, Func<byte[], object>> _recordConstructorDictionary = new Dictionary<Type, Func<byte[], object>>();

        static HashSet<ushort> _threadIds = new HashSet<ushort>();

        static Resource() {
            Resource.Is64bit = IntPtr.Size == 8;
        }

        public static bool Is64bit { get; private set; }

        public static ushort GetThreadId() {
            ushort result = 0;
            for (; ; ) {
                lock (_threadIds) {
                    if (!_threadIds.Contains(result)) {
                        _threadIds.Add(result);
                        return result;
                    }
                }
                unchecked { result++; }
            }
        }

        public static void RemoveThreadId(ushort threadId) {
            lock (_threadIds) {
                try { _threadIds.Remove(threadId); } catch { }
            }
        }

        public static Func<byte[], object> GetRecordConstructor(Type recordType) {
            if (!_recordConstructorDictionary.ContainsKey(recordType)) {
                if (recordType.BaseType.GetGenericTypeDefinition()!= typeof(Record<>)) {
                    throw new ArgumentException();
                }
                var recordConstructor = recordType.GetConstructor(new Type[] { typeof(byte[]) });
                if (recordConstructor == null) {
                    throw new InvalidDefinitionException();
                }
                var parameter = Expression.Parameter(typeof(byte[]));
                var newExpression = Expression.New(recordConstructor, parameter);
                var lambda = Expression.Lambda<Func<byte[], object>>(newExpression, parameter);
                _recordConstructorDictionary[recordType] = lambda.Compile();
            }
            return _recordConstructorDictionary[recordType];
        }

        public static FieldConverterAttribute GetFieldConverterAttribute(Type fieldConverterType) {
            if (fieldConverterType == null) {
                throw new ArgumentNullException();
            }
            if (!_fieldConverterAttributeDictionary.ContainsKey(fieldConverterType)) {
                var attribute = fieldConverterType.GetCustomAttributes(typeof(FieldConverterAttribute), false).SingleOrDefault() as FieldConverterAttribute;
                if (attribute == null) {
                    throw new ArgumentException();
                }
                _fieldConverterAttributeDictionary[fieldConverterType] = attribute;
            }
            return _fieldConverterAttributeDictionary[fieldConverterType];
        }

        public static KeyTypeAttribute GetKeyTypeAttribute(KeyType keyType) {
            if (!_keyTypeDictionary.ContainsKey(keyType)) {
                var type = keyType.GetType();
                var name = Enum.GetName(type, keyType);
                _keyTypeDictionary[keyType] = type.GetField(name)
                    .GetCustomAttributes(typeof(KeyTypeAttribute), false)
                    .SingleOrDefault() as KeyTypeAttribute;
            }
            return _keyTypeDictionary[keyType];
        }

        public static IFieldConverter GetFieldConverter(Type fieldConverterType) {
            if (fieldConverterType == null) {
                throw new ArgumentNullException();
            }
            if (!_fieldConverterDictionary.ContainsKey(fieldConverterType)) {
                var interfaceType = fieldConverterType.GetInterface(typeof(IFieldConverter).Name);
                if (interfaceType == null) {
                    throw new ArgumentException();
                }
                var constructor = fieldConverterType.GetConstructor(Type.EmptyTypes);
                if (constructor == null) {
                    throw new ArgumentException();
                } else {
                    _fieldConverterDictionary[fieldConverterType] = (IFieldConverter)constructor.Invoke(null);
                }
            }
            return _fieldConverterDictionary[fieldConverterType];
        }

        static void SetRecord(Type recordType) {
            if (recordType == null) {
                throw new ArgumentNullException();
            }
            if (!_recordDictionary.ContainsKey(recordType)) {
                _recordDictionary[recordType] = new RecordInfo(recordType);
            }
        }

        public static RecordInfo GetRecordInfo(Type recordType) {
            if (recordType == null) {
                throw new ArgumentNullException();
            }
            Resource.SetRecord(recordType);
            return _recordDictionary[recordType];
        }

        public static bool ContainsFieldInfoKey(MemberInfo memberInfo) {
            if (memberInfo == null) {
                throw new ArgumentNullException();
            }
            Resource.SetRecord(memberInfo.DeclaringType);
            return _fieldMemberInfoDictionary.ContainsKey(memberInfo);
        }

        public static FieldInfo GetFieldInfo(MemberInfo memberInfo) {
            if (memberInfo == null) {
                throw new ArgumentNullException();
            }
            Resource.SetRecord(memberInfo.DeclaringType);
            if (_fieldMemberInfoDictionary.ContainsKey(memberInfo)) {
                return _fieldMemberInfoDictionary[memberInfo];
            } else {
                return null;
            }
        }

        public static FieldInfo GetFieldInfo(Type recordType, string propertyName) {
            if (recordType == null || propertyName == null) {
                throw new ArgumentNullException();
            }
            Resource.SetRecord(recordType);
            var name = recordType.FullName + "." + propertyName;
            if (_fieldNameDictionary.ContainsKey(name)) {
                return _fieldNameDictionary[name];
            } else {
                return null;
            }
        }

        public static byte[] GetBuffer() {
            lock (Resource.BufferQueue) {
                if (Resource.BufferQueue.Count > 0) {
                    return Resource.BufferQueue.Dequeue();
                } else {
                    return new byte[ushort.MaxValue];
                }
            }
        }

        public static void Recycle(byte[] buffer) {
            lock (Resource.BufferQueue) {
                if (Resource.BufferQueue.Count < Config.TemporaryBufferQueueCapacity) {
                    Resource.BufferQueue.Enqueue(buffer);
                }
            }
        }
    }
}
