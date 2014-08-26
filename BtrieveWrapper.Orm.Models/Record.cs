using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
#if NET_3_5
using System.Windows;
#endif
using BtrieveWrapper.Orm;

namespace BtrieveWrapper.Orm.Models
{
    [Serializable]
    [XmlType(Namespace = "urn:BtrieveWrapperModelSchema")]
    public class Record 
#if NET_3_5
        : DependencyObject
#endif
    {
#if NET_3_5
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(Record));
        public static readonly DependencyProperty FixedLengthProperty = DependencyProperty.Register(
            "FixedLength", typeof(ushort), typeof(Record));
        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
            "PageSize", typeof(ushort), typeof(Record));
        public static readonly DependencyProperty DuplicatedPointerCountProperty = DependencyProperty.Register(
            "DuplicatedPointerCount", typeof(byte), typeof(Record));
        public static readonly DependencyProperty AllocationProperty = DependencyProperty.Register(
            "Allocation", typeof(ushort), typeof(Record));
        public static readonly DependencyProperty VariableOptionProperty = DependencyProperty.Register(
            "VariableOption", typeof(RecordVariableOption), typeof(Record));
        public static readonly DependencyProperty UsesIndexBalancingProperty = DependencyProperty.Register(
            "UsesIndexBalancing", typeof(bool), typeof(Record));
        public static readonly DependencyProperty IsCompressedProperty = DependencyProperty.Register(
            "IsCompressed", typeof(bool), typeof(Record));
        public static readonly DependencyProperty FreeSpaceThresholdProperty = DependencyProperty.Register(
            "FreeSpaceThreshold", typeof(FreeSpaceThreshold), typeof(Record));
        public static readonly DependencyProperty IsManualKeyNumberProperty = DependencyProperty.Register(
            "IsManualKeyNumber", typeof(bool), typeof(Record));
        public static readonly DependencyProperty SystemDataOptionProperty = DependencyProperty.Register(
            "SystemDataOption", typeof(SystemDataOption), typeof(Record));
        public static readonly DependencyProperty PrimaryKeyNumberProperty = DependencyProperty.Register(
            "PrimaryKeyNumber", typeof(sbyte), typeof(Record));
        public static readonly DependencyProperty DefaultByteProperty = DependencyProperty.Register(
            "DefaultByte", typeof(byte), typeof(Record));
        public static readonly DependencyProperty UriTableProperty = DependencyProperty.Register(
            "UriTable", typeof(string), typeof(Record));
        public static readonly DependencyProperty UriDbFileProperty = DependencyProperty.Register(
            "UriDbFile", typeof(string), typeof(Record));
        public static readonly DependencyProperty UriFileProperty = DependencyProperty.Register(
            "UriFile", typeof(string), typeof(Record));
        public static readonly DependencyProperty AbsolutePathProperty = DependencyProperty.Register(
            "AbsolutePath", typeof(string), typeof(Record));
        public static readonly DependencyProperty RelativePathProperty = DependencyProperty.Register(
            "RelativePath", typeof(string), typeof(Record));
        public static readonly DependencyProperty OwnerNameProperty = DependencyProperty.Register(
            "OwnerName", typeof(string), typeof(Record));
        public static readonly DependencyProperty OwnerNameOptionProperty = DependencyProperty.Register(
            "OwnerNameOption", typeof(OwnerNameOption), typeof(Record));
        public static readonly DependencyProperty OpenModeProperty = DependencyProperty.Register(
            "OpenMode", typeof(OpenMode), typeof(Record));
        public static readonly DependencyProperty DataBufferCapacityProperty = DependencyProperty.Register(
            "DataBufferCapacity", typeof(ushort), typeof(Record));
        public static readonly DependencyProperty VariableFieldCapacityProperty = DependencyProperty.Register(
            "VariableFieldCapacity", typeof(ushort), typeof(Record));
        public static readonly DependencyProperty RejectCountProperty = DependencyProperty.Register(
            "RejectCount", typeof(ushort), typeof(Record));
#endif
        public Record() {
            this.FixedLength = 100;
            this.PageSize = 4096;
            this.DuplicatedPointerCount = 0;
            this.Allocation = 0;
            this.VariableOption = RecordVariableOption.NotVariable;
            this.UsesIndexBalancing = false;
            this.IsCompressed = false;
            this.FreeSpaceThreshold = FreeSpaceThreshold.FivePercent;
            this.IsManualKeyNumber = true;
            this.SystemDataOption = BtrieveWrapper.SystemDataOption.None;
            this.PrimaryKeyNumber = 0;
            this.DefaultByte = 0;
            this.UriTable = null;
            this.UriDbFile = null;
            this.UriFile = null;
            this.AbsolutePath = null;
            this.RelativePath = null;
            this.OwnerName = null;
            this.OwnerNameOption = OwnerNameOption.NoEncryption;
            this.OpenMode = BtrieveWrapper.OpenMode.Normal;
            this.VariableFieldCapacity = 0;
            this.RejectCount = 0;

#if NET_3_5
            this.FieldCollection = new ObservableCollection<Field>();
            this.KeyCollection = new ObservableCollection<Key>();
            this.FieldCollection.CollectionChanged += FieldCollection_CollectionChanged;
            this.KeyCollection.CollectionChanged += KeyCollection_CollectionChanged;
#else
            this.FieldCollection = new List<Field>();
            this.KeyCollection = new List<Key>();
#endif
        }


#if NET_3_5
        void FieldCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                foreach (Field field in e.NewItems) {
                    field.Record = this;
                    if (field.Id == 0) {
                        field.Id = this.FieldCollection.Max(f => f.Id) + 1;
                    }
                }
            }
        }

        void KeyCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                foreach (Key key in e.NewItems) {
                    key.Record = this;
                }
            }
        }

        [XmlAttribute]
        public string Name { get { return (string)this.GetValue(NameProperty); } set { this.SetValue(NameProperty, value); } }
        [XmlAttribute]
        public ushort FixedLength { get { return (ushort)this.GetValue(FixedLengthProperty); } set { this.SetValue(FixedLengthProperty, value); } }
        [XmlAttribute]
        public ushort PageSize { get { return (ushort)this.GetValue(PageSizeProperty); } set { this.SetValue(PageSizeProperty, value); } }
        [XmlAttribute]
        public byte DuplicatedPointerCount { get { return (byte)this.GetValue(DuplicatedPointerCountProperty); } set { this.SetValue(DuplicatedPointerCountProperty, value); } }
        [XmlAttribute]
        public ushort Allocation { get { return (ushort)this.GetValue(AllocationProperty); } set { this.SetValue(AllocationProperty, value); } }
        [XmlAttribute]
        public RecordVariableOption VariableOption { get { return (RecordVariableOption)this.GetValue(VariableOptionProperty); } set { this.SetValue(VariableOptionProperty, value); } }
        [XmlAttribute]
        public bool UsesIndexBalancing { get { return (bool)this.GetValue(UsesIndexBalancingProperty); } set { this.SetValue(UsesIndexBalancingProperty, value); } }
        [XmlAttribute]
        public bool IsCompressed { get { return (bool)this.GetValue(IsCompressedProperty); } set { this.SetValue(IsCompressedProperty, value); } }
        [XmlAttribute]
        public FreeSpaceThreshold FreeSpaceThreshold { get { return (FreeSpaceThreshold)this.GetValue(FreeSpaceThresholdProperty); } set { this.SetValue(FreeSpaceThresholdProperty, value); } }
        [XmlAttribute]
        public bool IsManualKeyNumber { get { return (bool)this.GetValue(IsManualKeyNumberProperty); } set { this.SetValue(IsManualKeyNumberProperty, value); } }
        [XmlAttribute]
        public SystemDataOption SystemDataOption { get { return (SystemDataOption)this.GetValue(SystemDataOptionProperty); } set { this.SetValue(SystemDataOptionProperty, value); } }

        [XmlAttribute]
        public sbyte PrimaryKeyNumber { get { return (sbyte)this.GetValue(PrimaryKeyNumberProperty); } set { this.SetValue(PrimaryKeyNumberProperty, value); } }
        [XmlAttribute]
        public byte DefaultByte { get { return (byte)this.GetValue(DefaultByteProperty); } set { this.SetValue(DefaultByteProperty, value); } }

        [XmlAttribute]
        public string UriTable { get { return (string)this.GetValue(UriTableProperty); } set { this.SetValue(UriTableProperty, value); } }
        [XmlAttribute]
        public string UriDbFile { get { return (string)this.GetValue(UriDbFileProperty); } set { this.SetValue(UriDbFileProperty, value); } }
        [XmlAttribute]
        public string UriFile { get { return (string)this.GetValue(UriFileProperty); } set { this.SetValue(UriFileProperty, value); } }
        [XmlAttribute]
        public string AbsolutePath { get { return (string)this.GetValue(AbsolutePathProperty); } set { this.SetValue(AbsolutePathProperty, value); } }
        [XmlAttribute]
        public string RelativePath { get { return (string)this.GetValue(RelativePathProperty); } set { this.SetValue(RelativePathProperty, value); } }

        [XmlAttribute]
        public string OwnerName { get { return (string)this.GetValue(OwnerNameProperty); } set { this.SetValue(OwnerNameProperty, value); } }
        [XmlAttribute]
        public OwnerNameOption OwnerNameOption { get { return (OwnerNameOption)this.GetValue(OwnerNameOptionProperty); } set { this.SetValue(OwnerNameOptionProperty, value); } }
        [XmlAttribute]
        public OpenMode OpenMode { get { return (OpenMode)this.GetValue(OpenModeProperty); } set { this.SetValue(OpenModeProperty, value); } }
        [XmlAttribute]
        public ushort VariableFieldCapacity { get { return (ushort)this.GetValue(VariableFieldCapacityProperty); } set { this.SetValue(VariableFieldCapacityProperty, value); } }
        [XmlAttribute]
        public ushort RejectCount { get { return (ushort)this.GetValue(RejectCountProperty); } set { this.SetValue(RejectCountProperty, value); } }

        [XmlIgnore]
        public ObservableCollection<Field> FieldCollection { get; private set; }
        [XmlIgnore]
        public ObservableCollection<Key> KeyCollection { get; private set; }
#else
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public ushort FixedLength { get; set; }
        [XmlAttribute]
        public ushort PageSize { get; set; }
        [XmlAttribute]
        public byte DuplicatedPointerCount { get; set; }
        [XmlAttribute]
        public ushort Allocation { get; set; }
        [XmlAttribute]
        public RecordVariableOption VariableOption { get; set; }
        [XmlAttribute]
        public bool UsesIndexBalancing { get; set; }
        [XmlAttribute]
        public bool IsCompressed { get; set; }
        [XmlAttribute]
        public FreeSpaceThreshold FreeSpaceThreshold { get; set; }
        [XmlAttribute]
        public bool IsManualKeyNumber { get; set; }
        [XmlAttribute]
        public SystemDataOption SystemDataOption { get; set; }

        [XmlAttribute]
        public sbyte PrimaryKeyNumber { get; set; }
        [XmlAttribute]
        public byte DefaultByte { get; set; }

        [XmlAttribute]
        public string UriTable { get; set; }
        [XmlAttribute]
        public string UriDbFile { get; set; }
        [XmlAttribute]
        public string UriFile { get; set; }
        [XmlAttribute]
        public string AbsolutePath { get; set; }
        [XmlAttribute]
        public string RelativePath { get; set; }

        [XmlAttribute]
        public string OwnerName { get; set; }
        [XmlAttribute]
        public OwnerNameOption OwnerNameOption { get; set; }
        [XmlAttribute]
        public OpenMode OpenMode { get; set; }
        [XmlAttribute]
        public ushort VariableFieldCapacity { get; set; }
        [XmlAttribute]
        public ushort RejectCount { get; set; }

        [XmlIgnore]
        public List<Field> FieldCollection { get; private set; }
        [XmlIgnore]
        public List<Key> KeyCollection { get; private set; }
#endif

        [XmlArrayItem]
        public Field[] Fields {
            get { return this.FieldCollection.ToArray(); }
            set {
                this.FieldCollection.Clear();
                foreach (var field in value) {
                    this.FieldCollection.Add(field);
                }
            }
        }
        [XmlArrayItem]
        public Key[] Keys { get { return this.KeyCollection.ToArray(); } 
            set { 
                this.KeyCollection.Clear();
                foreach (var key in value) {
                    this.KeyCollection.Add(key);
                }
            } }

        [XmlIgnore]
        public Model Model { get; internal set; }
        [XmlIgnore]
        public int Number { get; internal set; }

        [XmlIgnore]
        public string DisplayName { get { return String.IsNullOrEmpty(this.Name) ? String.Format(Config.RecordName, this.Number) : this.Name; } }

        [XmlIgnore]
        public IEnumerable<KeyValuePair<string, object>> AttributeParameters {
            get {
                var attr = new RecordAttribute(0);
                if (this.PageSize != attr.PageSize) {
                    yield return new KeyValuePair<string, object>("PageSize", this.PageSize);
                }
                if (this.DuplicatedPointerCount != attr.DuplicatedPointerCount) {
                    yield return new KeyValuePair<string, object>("DuplicatedPointerCount", this.DuplicatedPointerCount);
                }
                if (this.Allocation != attr.Allocation) {
                    yield return new KeyValuePair<string, object>("Allocation", this.Allocation);
                }
                if (this.VariableOption != attr.VariableOption) {
                    yield return new KeyValuePair<string, object>("VariableOption", "BtrieveWrapper.RecordVariableOption." + this.Allocation);
                }
                if (this.UsesIndexBalancing != attr.UsesIndexBalancing) {
                    yield return new KeyValuePair<string, object>("UsesIndexBalancing", this.UsesIndexBalancing ? "true" : "false");
                }
                if (this.IsCompressed != attr.IsCompressed) {
                    yield return new KeyValuePair<string, object>("IsCompressed", this.IsCompressed ? "true" : "false");
                }
                if (this.FreeSpaceThreshold != attr.FreeSpaceThreshold) {
                    yield return new KeyValuePair<string, object>("FreeSpaceThreshold", "BtrieveWrapper.FreeSpaceThreshold." + this.FreeSpaceThreshold);
                }
                if (this.SystemDataOption != attr.SystemDataOption) {
                    yield return new KeyValuePair<string, object>("SystemDataOption", "BtrieveWrapper.SystemDataOption." + this.SystemDataOption);
                }
                if (this.PrimaryKeyNumber != attr.PrimaryKeyNumber) {
                    yield return new KeyValuePair<string, object>("PrimaryKeyNumber", this.PrimaryKeyNumber);
                }
                if (this.DefaultByte != attr.DefaultByte) {
                    yield return new KeyValuePair<string, object>("DefaultByte", this.DefaultByte);
                }
                if (this.Model.PathType != attr.PathType) {
                    yield return new KeyValuePair<string, object>("PathType", "BtrieveWrapper.Orm.PathType." + this.Model.PathType);
                }
                if (this.Model.UriHost != null) {
                    yield return new KeyValuePair<string, object>("UriHost", "\"" + this.Model.UriHost + "\"");
                }
                if (this.Model.UriUser != null) {
                    yield return new KeyValuePair<string, object>("UriUser", "\"" + this.Model.UriUser + "\"");
                }
                if (this.Model.UriDbName != null) {
                    yield return new KeyValuePair<string, object>("UriDbName", "\"" + this.Model.UriDbName + "\"");
                }
                if (this.UriTable != null) {
                    yield return new KeyValuePair<string, object>("UriTable", "\"" + this.UriTable + "\"");
                }
                if (this.UriDbFile != null) {
                    yield return new KeyValuePair<string, object>("UriDbFile", "\"" + this.UriDbFile + "\"");
                }
                if (this.UriFile != null) {
                    yield return new KeyValuePair<string, object>("UriFile", "\"" + this.UriFile + "\"");
                }
                if (this.Model.UriPassword != null) {
                    yield return new KeyValuePair<string, object>("UriPassword", "\"" + this.Model.UriPassword + "\"");
                }
                if (this.Model.UriPrompt != null &&(this.Model.UriPrompt=="true"||this.Model.UriPrompt=="false")) {
                    yield return new KeyValuePair<string, object>("UriPrompt", this.Model.UriPrompt);
                }
                if (this.AbsolutePath != null) {
                    yield return new KeyValuePair<string, object>("AbsolutePath", "@\"" + this.AbsolutePath + "\"");
                }
                if (this.Model.RelativeDirectory != null) {
                    yield return new KeyValuePair<string, object>("RelativeDirectory", "@\"" + this.Model.RelativeDirectory + "\"");
                }
                if (this.RelativePath != null) {
                    yield return new KeyValuePair<string, object>("RelativePath", "@\"" + this.RelativePath + "\"");
                }
                if (this.OwnerName != null) {
                    yield return new KeyValuePair<string, object>("OwnerName", "\"" + this.OwnerName + "\"");
                }
                if (this.OwnerNameOption != attr.OwnerNameOption) {
                    yield return new KeyValuePair<string, object>("OwnerNameOption", "BtrieveWrapper.OwnerNameOption." + this.OwnerNameOption);
                }
                if (this.OpenMode != attr.OpenMode) {
                    yield return new KeyValuePair<string, object>("OpenMode", "BtrieveWrapper.OpenMode." + this.OpenMode);
                }
                if (this.VariableFieldCapacity != attr.VariableFieldCapacity) {
                    yield return new KeyValuePair<string, object>("VariableFieldCapacity", this.VariableFieldCapacity);
                }
                if (this.RejectCount != attr.RejectCount) {
                    yield return new KeyValuePair<string, object>("RejectCount", this.RejectCount);
                }
                if (this.Model.DllPath != null) {
                    yield return new KeyValuePair<string, object>("DllPath", "\"" + this.Model.DllPath + "\"");
                }
            }
        }

        public static void Initialize(Record record, bool setPrimaryKey = false) {
            if (record == null) {
                throw new InvalidModelException();
            }
            if (record.Fields == null) {
                record.Fields = new Field[0];
            }
            if (record.FieldCollection.Count != record.FieldCollection.Select(f => f.Id).Distinct().Count()) {
                throw new InvalidModelException();
            }
            if (record.Keys == null) {
                record.Keys = new Key[0];
            }
            if (!record.Keys.All(k => k.KeyNumber >= 0 && k.KeyNumber <= 118)) {
                throw new InvalidModelException();
            }
            if (record.Keys.Length != record.Keys.Select(k => k.KeyNumber).Distinct().Count()) {
                throw new InvalidModelException();
            }
            foreach (var key in record.Keys) {
                if (key.Segments == null || key.Segments.Length == 0) {
                    throw new InvalidModelException();
                }
                ushort index = 0;
                foreach (var keySegment in key.Segments) {
                    if (keySegment.Index != index) {
                        throw new InvalidModelException();
                    }
                    index++;
                }
            }
            foreach (var key in record.Keys) {
                ushort position = 0;
                foreach (var keySegment in key.Segments) {
                    try {
                        if (keySegment.Field == null) {
                            keySegment.Field = record.Fields
                                .Single(f => f.Id == keySegment.FieldId);
                        } else {
                            keySegment.FieldId = record.Fields
                                .Single(f => f == keySegment.Field).Id;
                        }
                    } catch(Exception e) {
                        throw new InvalidModelException(e);
                    }
                    keySegment.KeyPosition = position;
                    position += keySegment.Field.Length;
                }
            }
            var ranges = record.Keys
                .SelectMany(k => k.Segments)
                .Select(s => new {
                    Position = s.Field.Position,
                    Length = s.Field.Length
                })
                .Distinct();
            foreach (var range in ranges) {
                if (!record.Fields.Any(f => f.Position == range.Position && f.Length == range.Length)) {
                    throw new InvalidModelException();
                }
            }
            foreach (var field in record.Fields) {
                field.KeySegments = record.Keys.SelectMany(k => k.Segments).Where(s => s.Field == field).ToArray();
            }
            if (setPrimaryKey) {
                var primaryKey = record.Keys
                    .FirstOrDefault(k => k.DuplicateKeyOption == DuplicateKeyOption.Unique);
                if (primaryKey == null) {
                    record.PrimaryKeyNumber = -1;
                } else {
                    record.PrimaryKeyNumber = primaryKey.KeyNumber;
                }
            } else {
                if (record.PrimaryKeyNumber != -1 &&
                    !record.Keys.Any(k => k.KeyNumber == record.PrimaryKeyNumber && k.DuplicateKeyOption == DuplicateKeyOption.Unique)) {
                    throw new InvalidModelException();
                }
            }
        }

        public static Record FromBtrieveFile(Path path, string dllPath = null, byte defaultByte = 0x00, string ownerName = null) {
            StatData stat;
            var nativeOperator = new Operator(dllPath);
            var positionBlock = nativeOperator.Open(path.GetFilePath(), ownerName);
            try {
                stat = nativeOperator.Stat(positionBlock);
            } finally {
                nativeOperator.Close(positionBlock);
            }

            var statKeySpecs = new List<StatKeySpec>();
            var keys = new List<Key>();
            var fields = new List<Field>();
            foreach (var keySpec in stat.KeySpecs) {
                statKeySpecs.Add(keySpec);
                if (!keySpec.IsSegmentKey) {
                    var statKey = statKeySpecs
                        .Select(s => new {
                            DuplicateKeyOption = s.DuplicateKeyOption,
                            IsModifiable = s.IsModifiable,
                            NullKeyOption = s.NullKeyOption,
                            KeyNumber = s.Number
                        })
                        .Distinct()
                        .Single();
                    var keySegments = new List<KeySegment>();
                    ushort i = 0;
                    foreach (var statKeySpec in statKeySpecs) {
                        var field = fields.SingleOrDefault(f => 
                            f.KeyType == statKeySpec.KeyType && 
                            f.Position == statKeySpec.Position && 
                            f.Length == statKeySpec.Length);
                        if (field == null) {
                            field = new Field() {
                                Position = statKeySpec.Position,
                                Length = statKeySpec.Length,
                                KeyType = statKeySpec.KeyType,
                                ConverterTypeName = Config.GetConverterTypeName(statKeySpec.KeyType, statKeySpec.Length),
                                Parameter = Config.GetConverterParameter(statKeySpec.KeyType, statKeySpec.Length),
                                NullType = NullType.None
                            };
                            fields.Add(field);
                        }
                        keySegments.Add(
                            new KeySegment() {
                                Index = i,
                                KeyType = statKeySpec.KeyType,
                                NullValue = statKeySpec.NullValue,
                                IsDescending = statKeySpec.IsDescending,
                                IsIgnoreCase = statKeySpec.IsIgnoreCase,
                                Field = field
                            });
                        i++;
                    }
                    var key = new Key();
                    key.Name = String.Format(Config.KeyName, statKey.KeyNumber);
                    key.DuplicateKeyOption = statKey.DuplicateKeyOption;
                    key.IsModifiable = statKey.IsModifiable;
                    key.NullKeyOption = statKey.NullKeyOption;
                    key.KeyNumber = statKey.KeyNumber;
                    key.Segments = keySegments.ToArray();
                    keys.Add(key);
                    statKeySpecs.Clear();
                }
            }
            if (statKeySpecs.Count != 0) {
                throw new InvalidModelException();
            }

            var result = new Record();
            if (path.PathType == PathType.Uri) {
                result.Name = path.UriTable ?? new System.IO.FileInfo(path.UriDbFile ?? path.UriFile ?? "Record").Name;
            } else {
                var fileInfo = new System.IO.FileInfo(path.GetFilePath());
                result.Name = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length).TrimEnd('.');
            }
            result.FixedLength = stat.FileSpec.RecordLength;
            result.PageSize = stat.FileSpec.PageSize;
            result.DuplicatedPointerCount = 0;
            result.Allocation = 0;
            result.VariableOption = stat.FileSpec.VariableOption;
            result.UsesIndexBalancing = stat.FileSpec.UsesIndexBalancing;
            result.IsCompressed = stat.FileSpec.IsCompressed;
            result.FreeSpaceThreshold = stat.FileSpec.FreeSpaceThreshold;
            result.IsManualKeyNumber = stat.FileSpec.IsManualKeyNumber;
            result.SystemDataOption = stat.FileSpec.SystemDataOption;
            result.DefaultByte = defaultByte;
            result.OwnerName = ownerName;
            result.UriTable = path.UriTable;
            result.UriDbFile = path.UriDbFile;
            result.UriFile = path.UriFile;
            result.AbsolutePath = path.AbsolutePath;
            result.RelativePath = path.RelativePath;
            result.VariableFieldCapacity = 0;

            result.Keys = keys.ToArray();
            result.Fields = fields.OrderBy(f => f.Position).ToArray();
            Record.Initialize(result, true);
            return result;
        }
    }
}
