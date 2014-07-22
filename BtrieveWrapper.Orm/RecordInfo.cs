using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class RecordInfo
    {
        FieldInfo[] _variableRangeFields = null;

        internal RecordInfo(Type recordType) {
            if (recordType.BaseType.GetGenericTypeDefinition() != typeof(Record<>)) {
                throw new InvalidDefinitionException();
            }
            var classAttributes = recordType.GetCustomAttributes(false);
            var attribute = classAttributes.SingleOrDefault(a => a is RecordAttribute) as RecordAttribute;
            if (attribute == null) {
                throw new InvalidDefinitionException();
            }

            this.Type = recordType;
            this.FixedLength = attribute.FixedLength;
            this.PageSize = attribute.PageSize;
            this.DuplicatedPointerCount = attribute.DuplicatedPointerCount;
            this.Allocation = attribute.Allocation;
            this.VariableOption = attribute.VariableOption;
            this.UsesIndexBalancing = attribute.UsesIndexBalancing;
            this.IsCompressed = attribute.IsCompressed;
            this.FreeSpaceThreshold = attribute.FreeSpaceThreshold;
            this.SystemDataOption = attribute.SystemDataOption;

            this.PrimaryKeyNumber = attribute.PrimaryKeyNumber;
            this.DefaultByte = attribute.DefaultByte;
            this.DllPath = attribute.DllPath;

            this.PathType = attribute.PathType;
            this.UriHost = attribute.UriHost;
            this.UriUser = attribute.UriUser;
            this.UriDbName = attribute.UriDbName;
            this.UriTable = attribute.UriTable;
            this.UriDbFile = attribute.UriDbFile;
            this.UriFile = attribute.UriFile;
            this.UriPassword = attribute.UriPassword;
            this.UriPrompt = attribute.UriPrompt;
            this.AbsolutePath = attribute.AbsolutePath;
            this.RelativeDirectory = attribute.RelativeDirectory;
            this.RelativePath = attribute.RelativePath;

            this.OwnerName = attribute.OwnerName;
            this.OwnerNameOption = attribute.OwnerNameOption;
            this.OpenMode = attribute.OpenMode;

            if (this.VariableOption == RecordVariableOption.NotVariable) {
                this.VariableFieldCapacity = 0;
            } else {
                this.VariableFieldCapacity = attribute.VariableFieldCapacity;
            }
            this.RejectCount = attribute.RejectCount;

            var properties = recordType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead || p.CanWrite);
            var fields = new List<FieldInfo>();
            foreach (var property in properties) {
                var propertyAttributes = property.GetCustomAttributes(false);
                var fieldAttribute = propertyAttributes
                    .SingleOrDefault(a => a is FieldAttribute) as FieldAttribute;
                if (fieldAttribute == null) {
                    continue;
                }
                if (fieldAttribute.Position + fieldAttribute.Length > attribute.FixedLength) {
                    throw new InvalidDefinitionException();
                }
                var field = new FieldInfo(fieldAttribute, this, propertyAttributes.Where(a => a is KeySegmentAttribute).Select(a => (KeySegmentAttribute)a), property);
                field.Name = property.Name;
                field.FullName = recordType.FullName + "." + property.Name;
                fields.Add(field);
                Resource._fieldMemberInfoDictionary[property] = field;
                if (property.CanRead) {
                    Resource._fieldMemberInfoDictionary[property.GetGetMethod()] = field;
                }
                if (property.CanWrite) {
                    Resource._fieldMemberInfoDictionary[property.GetSetMethod()] = field;
                }
                Resource._fieldNameDictionary[field.FullName] = field;
            }
            foreach (var field in fields) {
                CheckKeyTypeLength(field);
            }
            this.Fields = fields.ToArray();

            var keyNumbers = fields.SelectMany(f => f.KeySegments.Select(s => s.KeyNumber)).Distinct().OrderBy(n => n).ToArray();
            var keys = new List<KeyInfo>();
            var keyAttributes = classAttributes
                .Where(a => a is KeyAttribute)
                .Select(a => (KeyAttribute)a);
            foreach (var keyNumber in keyNumbers) {
                var keyAttribute = keyAttributes
                    .SingleOrDefault(a => a.KeyNumber == keyNumber);
                if (keyAttribute == null) {
                    keyAttribute = new KeyAttribute(keyNumber);
                }

                var keySegments = fields
                    .Where(f => f.KeySegments.Any(s => s.KeyNumber == keyNumber))
                    .Select(f => f.KeySegments.Single(s => s.KeyNumber == keyNumber))
                    .OrderBy(s => s.Index);
                if (keySegments.Count() == 0 ||
                    keySegments
                    .Select((s, i) => s.Index != i)
                    .Any(r => r)) {
                    throw new InvalidDefinitionException();
                }
                keySegments.Last().IsSegmentKey = false;
                var key = new KeyInfo(keyAttribute, keySegments, this);
                keys.Add(key);

                var matches = new bool[key.Length];
                foreach (var keySegment in keySegments) {
                    for (var i = 0; i < keySegment.Field.Length; i++) {
                        if (matches[keySegment.Position + i]) {
                            throw new InvalidDefinitionException();
                        }
                        matches[keySegment.Position + i] = true;
                    }
                }
            }
            this.Keys = new KeyCollection(keys);

            foreach (var field in this.Fields) {
                var keyTypeAttribute = Resource.GetKeyTypeAttribute(field.KeyType);
                if (keyTypeAttribute != null) {
                    if (!keyTypeAttribute.ValidateLength(field.Length)) {
                        throw new InvalidDefinitionException();
                    }
                }
                if (field.NullType == NullType.Nullable) {
                    var nullFlagField = this.Fields.SingleOrDefault(f =>
                        f.NullType == NullType.NullFlag &&
                        f.Position == field.Position - 1);
                    if (nullFlagField == null ||
                        !field.KeySegments.All(s => nullFlagField.KeySegments.Any(ns => ns.Key == s.Key && ns.Index == s.Index - 1))) {
                        throw new InvalidDefinitionException();
                    }
                    field.NullFlagField = nullFlagField;
                }
            }

            var isModArray = new bool[this.FixedLength];
            foreach (var keySegment in keys.Where(k => !k.IsModifiable).SelectMany(k => k.Segments)) {
                for (var i = 0; i < keySegment.Field.Length; i++) {
                    isModArray[keySegment.Field.Position + i] = true;
                }
            }
            foreach (var field in this.Fields) {
                for (var i = 0; i < field.Length; i++) {
                    if (isModArray[field.Position + i]) {
                        field.IsModifiable = false;
                        break;
                    }
                }
            }
        }

        public Type Type { get; private set; }
        public ushort FixedLength { get; private set; }
        public ushort PageSize { get; private set; }
        public byte DuplicatedPointerCount { get; private set; }
        public ushort Allocation { get; private set; }
        public RecordVariableOption VariableOption { get; private set; }
        public bool UsesIndexBalancing { get; private set; }
        public bool IsCompressed { get; private set; }
        public FreeSpaceThreshold FreeSpaceThreshold { get; private set; }
        public bool IsManualKeyNumber { get; private set; }
        public SystemDataOption SystemDataOption { get; private set; }

        public sbyte PrimaryKeyNumber { get; private set; }
        public byte DefaultByte { get; private set; }
        public string DllPath { get; private set; }

        public PathType PathType { get; private set; }
        public string UriHost { get; private set; }
        public string UriUser { get; private set; }
        public string UriDbName { get; private set; }
        public string UriTable { get; private set; }
        public string UriDbFile { get; private set; }
        public string UriFile { get; private set; }
        public string UriPassword { get; private set; }
        public bool? UriPrompt { get; private set; }
        public string AbsolutePath { get; private set; }
        public string RelativeDirectory { get; private set; }
        public string RelativePath { get; private set; }

        public string OwnerName { get; private set; }
        public OwnerNameOption OwnerNameOption { get; private set; }
        public OpenMode OpenMode { get; private set; }

        public ushort VariableFieldCapacity { get; private set; }
        public ushort RejectCount { get; private set; }

        public IEnumerable<FieldInfo> VariableRangeFields {
            get {
                if (_variableRangeFields == null) {
                    _variableRangeFields = this.Fields.Where(f => f.ConverterType == typeof(Converters.VariableRangeConverter)).OrderBy(f => f.Position).ToArray();
                }
                return _variableRangeFields.Select(f => f);
            }
        }

        public ushort DataBufferCapacity {
            get { return (ushort)(this.FixedLength + this.VariableFieldCapacity); }
        }

        public IEnumerable<FieldInfo> Fields { get; private set; }
        public KeyCollection Keys { get; private set; }

        internal CreateFileSpec GetCreateFileSpec() {
            var fileFlag = FileFlag.None;
            if (this.VariableOption != RecordVariableOption.NotVariable) {
                fileFlag |= FileFlag.VarRecs;
                if ((this.VariableOption & RecordVariableOption.BlankTruncation) == RecordVariableOption.BlankTruncation) {
                    fileFlag |= FileFlag.BlankTrunc;
                }
                if ((this.VariableOption & RecordVariableOption.VariabletailAllocationTable) == RecordVariableOption.VariabletailAllocationTable) {
                    fileFlag |= FileFlag.VatsSupport;
                }
            }
            if (this.Allocation != 0) {
                fileFlag |= FileFlag.PreAlloc;
            }
            if (this.IsCompressed) {
                fileFlag |= FileFlag.DataComp;
            }
            if (this.UsesIndexBalancing) {
                fileFlag |= FileFlag.BalancedKeys;
            }
            switch (this.FreeSpaceThreshold) {
                case FreeSpaceThreshold.FivePercent:
                    break;
                case FreeSpaceThreshold.TenPercent:
                    fileFlag |= FileFlag.Free10;
                    break;
                case FreeSpaceThreshold.TwentyPercent:
                    fileFlag |= FileFlag.Free20;
                    break;
                case FreeSpaceThreshold.ThirtyPercent:
                    fileFlag |= FileFlag.Free30;
                    break;
                default:
                    throw new InvalidDefinitionException();
            }
            switch (this.SystemDataOption) {
                case SystemDataOption.FollowEngine:
                    break;
                case SystemDataOption.Force:
                    fileFlag |= FileFlag.SystemDataIncluded;
                    break;
                case SystemDataOption.None:
                    fileFlag |= FileFlag.NoSystemDataIncluded;
                    break;
                default:
                    throw new InvalidDefinitionException();
            }
            if (this.DuplicatedPointerCount != 0) {
                fileFlag |= FileFlag.DupPtrs;
            }
            if (this.Keys.Select((key, index) => key.KeyNumber != index).Any(b => b)) {
                fileFlag |= FileFlag.SpecifyKeyNums;
            }
            return new CreateFileSpec(
                this.FixedLength,
                this.PageSize,
                fileFlag,
                this.DuplicatedPointerCount,
                this.Allocation);
        }

        public KeyInfo GetKey(sbyte keyNumber) {
            return this.Keys.SingleOrDefault(k => k.KeyNumber == keyNumber);
        }

        public KeyInfo GetPrimaryKey() {
            return this.GetKey(this.PrimaryKeyNumber);
        }

        static void CheckKeyTypeLength(FieldInfo field) {
            var length = field.Length;
            if (length == 0) {
                throw new InvalidDefinitionException();
            }
            switch (field.KeyType) {
                case KeyType.Autoincrement:
                    if (length != 2 && length != 4) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.BFloat:
                case KeyType.Float:
                    if (length != 4 && length != 8) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.Currency:
                case KeyType.Money:
                case KeyType.Timestamp:
                    if (length != 8) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.Date:
                case KeyType.Time:
                    if (length != 4) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.Integer:
                case KeyType.UnsignedBinary:
                    if (length != 1 && length != 2 && length != 4 && length != 8) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.Logical:
                    if (length != 1 && length != 2) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.LString:
                case KeyType.ZString:
                    if (length < 1 || length > 256) {
                        throw new InvalidDefinitionException();
                    }
                    break;
                case KeyType.Numericsts:
                    if (length == 1) {
                        throw new InvalidDefinitionException();
                    }
                    break;
            }
        }
    }
}
