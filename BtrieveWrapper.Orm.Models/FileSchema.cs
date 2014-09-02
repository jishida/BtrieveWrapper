namespace BtrieveWrapper.Orm.Models
{
    [BtrieveWrapper.Orm.Key(0)]
    [BtrieveWrapper.Orm.Key(1,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(97,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriTable = "X$File")]
    public class FileSchema : BtrieveWrapper.Orm.Record<FileSchema>
    {
        public FileSchema() {
            //Initialize record.
        }

		public FileSchema(byte[] dataBuffer) { }

        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter))]
        public System.UInt16 Id {
            get { return (System.UInt16)this.GetValue("Id"); }
            set { this.SetValue("Id", value); }
        }

        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(2, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Name {
            get { return (System.String)this.GetValue("Name"); }
            set { this.SetValue("Name", value); }
        }

        [BtrieveWrapper.Orm.Field(22, 64, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Loc {
            get { return (System.String)this.GetValue("Loc"); }
            set { this.SetValue("Loc", value); }
        }

        [BtrieveWrapper.Orm.Field(86, 1, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.ByteConverter))]
        public System.Byte Flags {
            get { return (System.Byte)this.GetValue("Flags"); }
            set { this.SetValue("Flags", value); }
        }

        [BtrieveWrapper.Orm.Field(87, 4, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Reserved {
            get { return (System.String)this.GetValue("Reserved"); }
            set { this.SetValue("Reserved", value); }
        }

        public bool IsSchema {
            get { return (this.Flags & 0x10) == 0x10; }
        }

        
    }

    public class FileSchemaKeyCollection : BtrieveWrapper.Orm.KeyCollection<FileSchema> {
        public FileSchemaKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }
    }
}