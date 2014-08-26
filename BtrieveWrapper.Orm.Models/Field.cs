using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
#if NET_3_5
using System.Windows;
#endif
namespace BtrieveWrapper.Orm.Models
{
    [Serializable]
    [XmlType(Namespace = "urn:BtrieveWrapperModelSchema")]
    public class Field 
#if NET_3_5
        : DependencyObject
#endif
    {
#if NET_3_5
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            "Id", typeof(int), typeof(Field));
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(Field));
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(ushort), typeof(Field));
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length", typeof(ushort), typeof(Field));
        public static readonly DependencyProperty ConverterTypeProperty = DependencyProperty.Register(
            "ConverterType", typeof(Type), typeof(Field));
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter", typeof(string), typeof(Field));
        public static readonly DependencyProperty NullTypeProperty = DependencyProperty.Register(
            "NullType", typeof(NullType), typeof(Field));
        public static readonly DependencyProperty KeyTypeProperty = DependencyProperty.Register(
            "KeyType", typeof(KeyType), typeof(Field));
#endif
        public Field() {
            this.Id = 0;
            this.Position = 0;
            this.Length = 4;
            this.ConverterType = typeof(Converters.ByteArrayConverter);
            this.Parameter = null;
            this.NullType = Orm.NullType.None;
            this.KeyType = BtrieveWrapper.KeyType.String;
        }

#if NET_3_5
        [XmlAttribute]
        public int Id { get { return (int)this.GetValue(IdProperty); } set { this.SetValue(IdProperty, value); } }
        [XmlAttribute]
        public string Name { get { return (string)this.GetValue(NameProperty); } set { this.SetValue(NameProperty, value); } }
        [XmlAttribute]
        public ushort Position { get { return (ushort)this.GetValue(PositionProperty); } set { this.SetValue(PositionProperty, value); } }
        [XmlAttribute]
        public ushort Length { get { return (ushort)this.GetValue(LengthProperty); } set { this.SetValue(LengthProperty, value); } }
        [XmlAttribute]
        public string Parameter { get { return (string)this.GetValue(ParameterProperty); } set { this.SetValue(ParameterProperty, value); } }
        [XmlAttribute]
        public KeyType KeyType { get { return (KeyType)this.GetValue(KeyTypeProperty); } set { this.SetValue(KeyTypeProperty, value); } }
        [XmlAttribute]
        public NullType NullType { get { return (NullType)this.GetValue(NullTypeProperty); } set { this.SetValue(NullTypeProperty, value); } }

        [XmlIgnore]
        public Type ConverterType {
            get {
                return (Type)this.GetValue(ConverterTypeProperty);
            }
            set {
                this.SetValue(ConverterTypeProperty, value);
            }
        }
#else
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public ushort Position { get; set; }
        [XmlAttribute]
        public ushort Length { get; set; }
        [XmlAttribute]
        public string Parameter { get; set; }
        [XmlAttribute]
        public KeyType KeyType { get; set; }
        [XmlAttribute]
        public NullType NullType { get; set; }
        [XmlIgnore]
        public Type ConverterType { get; set; }
#endif
        [XmlAttribute]
        public string ConverterTypeName {
            get { return this.ConverterType.Assembly.GetName().Name + "," + this.ConverterType.FullName; }
            set {
                try {
                    this.ConverterType = Config.GetType(value);
                } catch {
                    this.ConverterType = Config.DefaultConverterType;
                }
            }
        }

        [XmlIgnore]
        public Type ValueType {
            get {
                try {
                    var result = ((BtrieveWrapper.Orm.FieldConverterAttribute)this.ConverterType
                        .GetCustomAttributes(typeof(BtrieveWrapper.Orm.FieldConverterAttribute), false)
                        .Single())
                            .ConvertType;
                    if (this.NullType == NullType.Nullable &&
                        result.IsValueType) {
                        result = typeof(Nullable<>).MakeGenericType(result);
                    }
                    return result;
                } catch {
                    return null;
                }
            }
        }

        [XmlIgnore]
        public string ValueTypeDisplayName {
            get {
                return GetDisplayName(this.ValueType);
            }
        }

        static string GetDisplayName(Type valueType) {
                if (valueType == null) {
                    return null;
                } else {
                    if (valueType.IsGenericType) {
                        var genericType = valueType.FullName.Split('`')[0];
                        var argumentTypes = valueType.GetGenericArguments();
                        var result = new StringBuilder();
                        result.Append(genericType);
                        result.Append("<");
                        for (var i = 0; i < argumentTypes.Length; i++) {
                            result.Append(GetDisplayName(argumentTypes[i]));
                            if (i != argumentTypes.Length - 1) {
                                result.Append(", ");
                            }
                        }
                        result.Append(">");
                        return result.ToString();
                    } else {
                        return valueType.FullName;
                    }
                }
        }

        [XmlIgnore]
        public string DisplayName { get { return String.IsNullOrEmpty(this.Name) ? String.Format(Config.FieldName, this.Id) : this.Name; } }

        [XmlIgnore]
        public Record Record { get; internal set; }
        [XmlIgnore]
        public IEnumerable<KeySegment> KeySegments { get; internal set; }

        public override string ToString() {
            return this.Name ?? "Field";
        }
    }
}
