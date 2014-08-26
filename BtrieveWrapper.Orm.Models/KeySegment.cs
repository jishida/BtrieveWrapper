using System;
using System.Collections.Generic;
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
    public class KeySegment 
#if NET_3_5
        : DependencyObject
#endif
    {
#if NET_3_5
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
            "Index", typeof(ushort), typeof(KeySegment));
        public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(
            "NullValue", typeof(byte), typeof(KeySegment));
        public static readonly DependencyProperty IsDescendingProperty = DependencyProperty.Register(
            "IsDescending", typeof(bool), typeof(KeySegment));
        public static readonly DependencyProperty IsIgnoreCaseProperty = DependencyProperty.Register(
            "IsIgnoreCase", typeof(bool), typeof(KeySegment));
        public static readonly DependencyProperty FieldProperty = DependencyProperty.Register(
            "Field", typeof(Field), typeof(KeySegment));
#endif

        public KeySegment() {
            this.Index = 0;
            this.NullValue = 0;
            this.IsDescending = false;
            this.IsIgnoreCase = false;
            this.Field = null;
        }

#if NET_3_5
        [XmlAttribute]
        public ushort Index { get { return (ushort)this.GetValue(IndexProperty); } set { this.SetValue(IndexProperty, value); } }
        [XmlAttribute]
        public byte NullValue { get { return (byte)this.GetValue(NullValueProperty); } set { this.SetValue(NullValueProperty, value); } }
        [XmlAttribute]
        public bool IsDescending { get { return (bool)this.GetValue(IsDescendingProperty); } set { this.SetValue(IsDescendingProperty, value); } }
        [XmlAttribute]
        public bool IsIgnoreCase { get { return (bool)this.GetValue(IsIgnoreCaseProperty); } set { this.SetValue(IsIgnoreCaseProperty, value); } }

        [XmlIgnore]
        public Field Field { get { return (Field)this.GetValue(FieldProperty); } set { this.SetValue(FieldProperty, value); } }
#else
        [XmlAttribute]
        public ushort Index { get; set; }
        [XmlAttribute]
        public byte NullValue { get; set; }
        [XmlAttribute]
        public bool IsDescending { get; set; }
        [XmlAttribute]
        public bool IsIgnoreCase { get; set; }

        [XmlIgnore]
        public Field Field { get; set; }
#endif
        [XmlAttribute]
        public int FieldId { get; set; }

        [XmlIgnore]
        public ushort KeyPosition { get; internal set; }
        [XmlIgnore]
        public Key Key { get; internal set; }

        [XmlIgnore]
        public KeyType KeyType { get; internal set; }

        [XmlIgnore]
        public IEnumerable<KeyValuePair<string, object>> AttributeParameters {
            get {
                var attr = new KeySegmentAttribute(0, 0);
                if (this.NullValue != attr.NullValue) {
                    yield return new KeyValuePair<string, object>("NullValue", this.NullValue);
                }
                if (this.IsDescending) {
                    yield return new KeyValuePair<string, object>("IsDescending", "true" );
                }
                if (this.IsIgnoreCase) {
                    yield return new KeyValuePair<string, object>("IsIgnoreCase", "true");
                }
            }
        }
    }
}
