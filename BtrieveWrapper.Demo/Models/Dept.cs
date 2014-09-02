namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(2,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(64,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Dept")]
    public class Dept : BtrieveWrapper.Orm.Record<Dept>
    {
        public Dept() {
            //Initialize record.
        }

		public Dept(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(0, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Name {
            get { return (System.String)this.GetValue("Name"); }
            set { this.SetValue("Name", value); }
        }
		
        [BtrieveWrapper.Orm.Field(20, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Phone_Number {
            get { return (System.Boolean)this.GetValue("N_Phone_Number"); }
            set { this.SetValue("N_Phone_Number", value); }
        }
		
        [BtrieveWrapper.Orm.Field(21, 6, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Phone_Number {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Phone_Number"); }
            set { this.SetValue("Phone_Number", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(27, 25, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Building_Name {
            get { return (System.String)this.GetValue("Building_Name"); }
            set { this.SetValue("Building_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 1)]
        [BtrieveWrapper.Orm.Field(52, 4, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt32Converter))]
        public System.UInt32 Room_Number {
            get { return (System.UInt32)this.GetValue("Room_Number"); }
            set { this.SetValue("Room_Number", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 0)]
        [BtrieveWrapper.Orm.Field(56, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 Head_Of_Dept {
            get { return (System.UInt64)this.GetValue("Head_Of_Dept"); }
            set { this.SetValue("Head_Of_Dept", value); }
        }
    }

    public class DeptKeyCollection : BtrieveWrapper.Orm.KeyCollection<Dept> {
        public DeptKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }

        public BtrieveWrapper.Orm.KeyInfo Key2 { get { return this[2]; } }
    }
}