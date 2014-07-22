using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows;

namespace BtrieveWrapper.Orm.Models
{
    [Serializable]
    [XmlRoot(Namespace = "urn:BtrieveWrapperModelSchema", IsNullable = false)]
    public class Model : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(Model));
        public static readonly DependencyProperty DllPathProperty = DependencyProperty.Register(
            "DllPath", typeof(string), typeof(Model));
        public static readonly DependencyProperty NamespaceProperty = DependencyProperty.Register(
            "Namespace", typeof(string), typeof(Model));
        public static readonly DependencyProperty PathTypeProperty = DependencyProperty.Register(
            "PathType", typeof(BtrieveWrapper.Orm.PathType), typeof(Model));
        public static readonly DependencyProperty UriHostProperty = DependencyProperty.Register(
            "UriHost", typeof(string), typeof(Model));
        public static readonly DependencyProperty UriUserProperty = DependencyProperty.Register(
            "UriUser", typeof(string), typeof(Model));
        public static readonly DependencyProperty UriDbNameProperty = DependencyProperty.Register(
            "UriDbName", typeof(string), typeof(Model));
        public static readonly DependencyProperty UriPasswordProperty = DependencyProperty.Register(
            "UriPassword", typeof(string), typeof(Model));
        public static readonly DependencyProperty UriPromptProperty = DependencyProperty.Register(
            "UriPrompt", typeof(string), typeof(Model));
        public static readonly DependencyProperty RelativeDirectoryProperty = DependencyProperty.Register(
            "RelativeDirectory", typeof(string), typeof(Model));


        public Model() {
            this.RecordCollection = new ObservableCollection<Record>();
            this.Name = "NewModel";
            this.DllPath = null;
            this.Namespace = "BtrieveWrapper.Orm.CustomModels";
            this.PathType = Orm.PathType.Uri;
            this.UriHost = null;
            this.UriUser = null;
            this.UriDbName = null;
            this.UriPassword = null;
            this.UriPrompt = null;
            this.RelativeDirectory = null;

            this.RecordCollection.CollectionChanged += RecordCollection_CollectionChanged;
        }

        void RecordCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                foreach (Record record in e.NewItems) {
                    record.Model = this;
                }
            }
        }

        [XmlAttribute]
        public string Name { get { return (string)this.GetValue(NameProperty); } set { this.SetValue(NameProperty, value); } }
        [XmlAttribute]
        public string DllPath { get { return (string)this.GetValue(DllPathProperty); } set { this.SetValue(DllPathProperty, value); } }
        [XmlAttribute]
        public string Namespace { get { return (string)this.GetValue(NamespaceProperty); } set { this.SetValue(NamespaceProperty, value); } }

        [XmlAttribute]
        public BtrieveWrapper.Orm.PathType PathType { get { return (BtrieveWrapper.Orm.PathType)this.GetValue(PathTypeProperty); } set { this.SetValue(PathTypeProperty, value); } }
        [XmlAttribute]
        public string UriHost { get { return (string)this.GetValue(UriHostProperty); } set { this.SetValue(UriHostProperty, value); } }
        [XmlAttribute]
        public string UriUser { get { return (string)this.GetValue(UriUserProperty); } set { this.SetValue(UriUserProperty, value); } }
        [XmlAttribute]
        public string UriDbName { get { return (string)this.GetValue(UriDbNameProperty); } set { this.SetValue(UriDbNameProperty, value); } }
        [XmlAttribute]
        public string UriPassword { get { return (string)this.GetValue(UriPasswordProperty); } set { this.SetValue(UriPasswordProperty, value); } }
        [XmlAttribute]
        public string UriPrompt { get { return (string)this.GetValue(UriPromptProperty); } set { this.SetValue(UriPromptProperty, value); } }
        [XmlAttribute]
        public string RelativeDirectory { get { return (string)this.GetValue(RelativeDirectoryProperty); } set { this.SetValue(RelativeDirectoryProperty, value); } }

        [XmlArrayItem]
        public Record[] Records {
            get { return this.RecordCollection.ToArray(); }
            set {
                this.RecordCollection.Clear();
                foreach (var record in value) {
                    this.RecordCollection.Add(record);
                }
            }
        }

        [XmlIgnore]
        public string DisplayName { get { return String.IsNullOrWhiteSpace(this.Name) ? Config.DefaultModelName : this.Name; } }

        [XmlIgnore]
        public ObservableCollection<Record> RecordCollection { get; private set; }

        public static Model FromDirectory(string name, string directory, string searchPattern = null, SearchOption searchOption = SearchOption.AllDirectories, string ownerName = null, string dllPath = null, string nameSpace = null) {
            if (searchPattern == null) {
                searchPattern = "*";
            }
            var directoryInfo = new DirectoryInfo(directory);

            var records = new List<Record>();
            foreach (var file in directoryInfo.EnumerateFiles(searchPattern, searchOption)) {
                var filePath = file.FullName.Substring(directoryInfo.FullName.Length).TrimStart('\\');
                try {
                    var record = Record.FromBtrieveFile(Path.Relative(filePath, directoryInfo.FullName), dllPath, ownerName: ownerName);
                    record.RelativePath = filePath;
                    records.Add(record);
                } catch (OperationException e) {
                    if (e.StatusCode != 12 && e.StatusCode != 30) {
                        throw;
                    }
                }
            }
            var result = new Model();
            result.Name = name;
            result.PathType = PathType.Relative;
            result.RelativeDirectory = directoryInfo.FullName;
            result.DllPath = dllPath;
            result.Records = records.ToArray();
            result.Namespace = nameSpace ?? "BtrieveWrapper.Orm.Models.CustomModels";
            var i = 0;
            foreach (var record in result.Records) {
                Record.Initialize(record);
                record.Number = i++;
            }
            return result;
        }

        public static bool ToXml(Model model, Stream stream) {
            var serializer = new XmlSerializer(typeof(Model));
            try {
                serializer.Serialize(stream, model);
            } catch {
                return false;
            }
            return true;
        }

        public static string ToXml(Model model) {
            using (var stream = new MemoryStream()) {
                Model.ToXml(model, stream);
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static bool ToXml(Model model, string path) {
            try {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    var serializer = new XmlSerializer(typeof(Model));
                    serializer.Serialize(stream, model);
                }
            } catch {
                return false;
            }
            return true;
        }

        public static Model FromXml(Stream stream) {
            Model result = null;
            var serializer = new XmlSerializer(typeof(Model));
            try {
                result = (Model)serializer.Deserialize(stream);
            } catch { }
            if (result == null || result.Records == null) {
                throw new InvalidModelException();
            }
            var i = 0;
            foreach (var record in result.Records) {
                Record.Initialize(record);
                record.Number = i++;
            }
            return result;
        }

        public static Model FromXml(string path) {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return Model.FromXml(stream);
            }
        }

        public static Model FromDdf(string uriHost, string uriDbName, string uriUser = null, string uriPassword = null, bool? uriPrompt = null, string ownerName = null, string dllPath = null, string nameSpace = null) {
            Model result = new Model();
            using (var client = new SchemaDbClient(dllPath))
            using (var fieldSchemaManager = client.FieldSchema(Path.Uri(uriHost, uriDbName, uriUser: uriUser, uriPassword: uriPassword, uriPrompt: uriPrompt), ownerName))
            using (var fileSchemaManager = client.FileSchema(Path.Uri(uriHost, uriDbName, uriUser: uriUser, uriPassword: uriPassword, uriPrompt: uriPrompt), ownerName)) {
                var fieldSchemas = fieldSchemaManager.Query().Where(s => s.DataType < 227).ToArray(); ;
                var fileSchemas = fileSchemaManager.Query().Where(s => !s.IsSchema).ToArray();
                var records = new List<Record>();
                foreach (var fileSchema in fileSchemas) {
                    var record = Record.FromBtrieveFile(Path.Uri(uriHost, uriDbName, fileSchema.Name, uriUser: uriUser, uriPassword: uriPassword, uriPrompt: uriPrompt), dllPath, ownerName: ownerName);
                    var newFields = new List<Field>();
                    foreach (var fieldSchema in fieldSchemas.Where(s => s.FileId == fileSchema.Id)) {
                        var field = record.Fields
                            .SingleOrDefault(f => f.Position == fieldSchema.Offset && f.Length == fieldSchema.Size && f.KeyType == fieldSchema.KeyType.Value);
                        if (field == null) {
                            field = new Field();
                            field.Position = fieldSchema.Offset;
                            field.Length = fieldSchema.Size;
                            field.ConverterTypeName = Config.GetConverterTypeName(fieldSchema.KeyType.Value, fieldSchema.Size, fieldSchema.Dec);
                            field.KeyType = fieldSchema.KeyType.Value;
                            field.Parameter = Config.GetConverterParameter(fieldSchema.KeyType.Value, fieldSchema.Size, fieldSchema.Dec);
                            newFields.Add(field);
                        }
                        field.NullType = fieldSchema.IsNullable ? NullType.Nullable : NullType.None;
                        field.Name = fieldSchema.Name;

                        if (field.NullType == NullType.Nullable) {
                            field = record.Fields
                                .SingleOrDefault(f => f.Position == fieldSchema.Offset - 1 && f.Length == 1 && f.KeyType == KeyType.LegacyString);
                            if (field != null) {
                                field.NullType = NullType.NullFlag;
                                field.ConverterType = typeof(Orm.Converters.NullFlagConverter);
                                if (field.Name == null) {
                                    field.Name = "N_" + fieldSchema.Name;
                                }
                            } else {
                                field = new Field() {
                                    Position = (ushort)(fieldSchema.Offset - 1),
                                    Name = "N_" + fieldSchema.Name,
                                    Length = 1,
                                    ConverterType = typeof(Orm.Converters.NullFlagConverter),
                                    Parameter = null,
                                    KeyType = BtrieveWrapper.KeyType.LegacyString,
                                    NullType = NullType.NullFlag,
                                };
                                newFields.Add(field);
                            }
                        }
                    }
                    foreach (var field in newFields) {
                        record.FieldCollection.Add(field);
                    }
                    record.Fields = record.Fields.OrderBy(f => f.Position).ToArray();
                    if (record.VariableOption != RecordVariableOption.NotVariable) {
                        var capacity = record.Fields.Max(f => f.Position + f.Length);
                        if (capacity > record.FixedLength) {

                        }
                        record.VariableFieldCapacity = (ushort)(capacity - record.FixedLength);
                    }
                    records.Add(record);
                }
                result.DllPath = dllPath;
                result.Name = uriDbName;
                result.PathType = PathType.Uri;
                result.UriHost = uriHost;
                result.UriDbName = uriDbName;
                result.UriUser = uriUser;
                result.UriPassword = uriPassword;
                result.UriPrompt = uriPrompt.HasValue ? uriPrompt.Value ? "true" : "false" : null;
                result.Namespace = nameSpace ?? "BtrieveWrapper.Orm.Models.CustomModels";
                result.Records = records.ToArray();
            }
            return result;
        }
    }
}
