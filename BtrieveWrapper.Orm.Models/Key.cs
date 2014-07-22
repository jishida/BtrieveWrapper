using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows;

using BtrieveWrapper.Orm;

namespace BtrieveWrapper.Orm.Models
{
    [Serializable]
    [XmlType(Namespace = "urn:BtrieveWrapperModelSchema")]
    public class Key : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(Key));
        public static readonly DependencyProperty KeyNumberProperty = DependencyProperty.Register(
            "KeyNumber", typeof(sbyte), typeof(Key));
        public static readonly DependencyProperty DuplicateKeyOptionProperty = DependencyProperty.Register(
            "DuplicateKeyOption", typeof(DuplicateKeyOption), typeof(Key));
        public static readonly DependencyProperty IsModifiableProperty = DependencyProperty.Register(
            "IsModifiable", typeof(bool), typeof(Key));
        public static readonly DependencyProperty NullKeyOptionProperty = DependencyProperty.Register(
            "NullKeyOption", typeof(NullKeyOption), typeof(Key));

        public Key() {
            this.KeySegmentCollection = new ObservableCollection<KeySegment>();
            this.KeyNumber = 0;
            this.DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.Unique;
            this.IsModifiable = false;
            this.NullKeyOption = BtrieveWrapper.NullKeyOption.None;

            this.KeySegmentCollection.CollectionChanged+=KeySegmentCollection_CollectionChanged;
        }

        void KeySegmentCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                foreach (KeySegment keySegment in e.NewItems) {
                    keySegment.Key = this;
                }
            }
            ushort index = 0;
            foreach (var keySegment in this.KeySegmentCollection) {
                keySegment.Index = index++;
            }
        }

        [XmlAttribute]
        public string Name { get { return (string)this.GetValue(NameProperty); } set { this.SetValue(NameProperty, value); } }
        [XmlAttribute]
        public sbyte KeyNumber { get { return (sbyte)this.GetValue(KeyNumberProperty); } set { this.SetValue(KeyNumberProperty, value); } }
        [XmlAttribute]
        public DuplicateKeyOption DuplicateKeyOption { get { return (DuplicateKeyOption)this.GetValue(DuplicateKeyOptionProperty); } set { this.SetValue(DuplicateKeyOptionProperty, value); } }
        [XmlAttribute]
        public bool IsModifiable { get { return (bool)this.GetValue(IsModifiableProperty); } set { this.SetValue(IsModifiableProperty, value); } }
        [XmlAttribute]
        public NullKeyOption NullKeyOption { get { return (NullKeyOption)this.GetValue(NullKeyOptionProperty); } set { this.SetValue(NullKeyOptionProperty, value); } }

        [XmlArrayItem]
        public KeySegment[] Segments { get { return this.KeySegmentCollection.ToArray(); } 
            set {
                this.KeySegmentCollection.Clear();
                foreach (var keySegment in value.OrderBy(s=>s.Index)) {
                    this.KeySegmentCollection.Add(keySegment);
                }
            } }

        [XmlIgnore]
        public Record Record { get; internal set; }

        [XmlIgnore]
        public string DisplayName { get { return String.IsNullOrWhiteSpace(this.Name) ? String.Format(Config.KeyName, this.KeyNumber) : this.Name; } }

        [XmlIgnore]
        public ObservableCollection<KeySegment> KeySegmentCollection { get; private set; }

        [XmlIgnore]
        public IEnumerable<KeyValuePair<string, object>> AttributeParameters {
            get {
                var attr = new KeyAttribute(0);
                if (this.DuplicateKeyOption != attr.DuplicateKeyOption) {
                    yield return new KeyValuePair<string, object>("DuplicateKeyOption", "BtrieveWrapper.DuplicateKeyOption." + this.DuplicateKeyOption);
                }
                if (this.IsModifiable != attr.IsModifiable) {
                    yield return new KeyValuePair<string, object>("IsModifiable", this.IsModifiable ? "true" : "false");
                }
                if (this.NullKeyOption != attr.NullKeyOption) {
                    yield return new KeyValuePair<string, object>("NullKeyOption", "BtrieveWrapper.NullKeyOption." + this.NullKeyOption);
                }
            }
        }
    }
}
