using System;
using System.Linq;

namespace BtrieveWrapper.Orm.Models
{
    [BtrieveWrapper.Orm.Key(0)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.LinkDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(2,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.LinkDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(3,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(4,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.LinkDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(32,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriTable = "X$Field")]
    public class FieldSchema : BtrieveWrapper.Orm.Record<FieldSchema>
    {
        public FieldSchema() {
            //Initialize record.
        }

		public FieldSchema(byte[] dataBuffer) { }

        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter))]
        public System.UInt16 Id {
            get { return (System.UInt16)this.GetValue("Id"); }
            set { this.SetValue("Id", value); }
        }

        [BtrieveWrapper.Orm.KeySegment(1, 0)]
        [BtrieveWrapper.Orm.KeySegment(3, 0)]
        [BtrieveWrapper.Orm.KeySegment(4, 0)]
        [BtrieveWrapper.Orm.Field(2, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter))]
        public System.UInt16 FileId {
            get { return (System.UInt16)this.GetValue("FileId"); }
            set { this.SetValue("FileId", value); }
        }

        [BtrieveWrapper.Orm.KeySegment(2, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.KeySegment(3, 1,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(4, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Name {
            get { return (System.String)this.GetValue("Name"); }
            set { this.SetValue("Name", value); }
        }

        [BtrieveWrapper.Orm.Field(24, 1, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.ByteConverter))]
        public System.Byte DataType {
            get { return (System.Byte)this.GetValue("DataType"); }
            set { this.SetValue("DataType", value); }
        }

        [BtrieveWrapper.Orm.KeySegment(4, 1)]
        [BtrieveWrapper.Orm.Field(25, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter))]
        public System.UInt16 Offset {
            get { return (System.UInt16)this.GetValue("Offset"); }
            set { this.SetValue("Offset", value); }
        }

        [BtrieveWrapper.Orm.Field(27, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter))]
        public System.UInt16 Size {
            get { return (System.UInt16)this.GetValue("Size"); }
            set { this.SetValue("Size", value); }
        }

        [BtrieveWrapper.Orm.KeySegment(4, 2)]
        [BtrieveWrapper.Orm.Field(29, 1, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.ByteConverter))]
        public System.Byte Dec {
            get { return (System.Byte)this.GetValue("Dec"); }
            set { this.SetValue("Dec", value); }
        }

        [BtrieveWrapper.Orm.Field(30, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter))]
        public System.UInt16 Flags {
            get { return (System.UInt16)this.GetValue("Flags"); }
            set { this.SetValue("Flags", value); }
        }

        public bool IsNullable {
            get { return (this.Flags & 0x04) == 0x04; }
        }

        public BtrieveWrapper.KeyType? KeyType {
            get { if (this.DataType < 227) { return (KeyType)this.DataType; } else { return null; } }
        }
    }

    public class FieldSchemaKeyCollection : BtrieveWrapper.Orm.KeyCollection<FieldSchema>
    {
        public FieldSchemaKeyCollection() : base() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }

        public BtrieveWrapper.Orm.KeyInfo Key2 { get { return this[2]; } }

        public BtrieveWrapper.Orm.KeyInfo Key3 { get { return this[3]; } }

        public BtrieveWrapper.Orm.KeyInfo Key4 { get { return this[4]; } }
    }
}