namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(2,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(87,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Faculty")]
    public class Faculty : BtrieveWrapper.Orm.Record<Faculty>
    {
        public Faculty() {
            //Initialize record.
        }

		public Faculty(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 ID {
            get { return (System.UInt64)this.GetValue("ID"); }
            set { this.SetValue("ID", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(8, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Dept_Name {
            get { return (System.Boolean)this.GetValue("N_Dept_Name"); }
            set { this.SetValue("N_Dept_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 1,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(9, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Dept_Name {
            get { return (System.String)this.GetValue("Dept_Name"); }
            set { this.SetValue("Dept_Name", value); }
        }
		
        [BtrieveWrapper.Orm.Field(29, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Designation {
            get { return (System.Boolean)this.GetValue("N_Designation"); }
            set { this.SetValue("N_Designation", value); }
        }
		
        [BtrieveWrapper.Orm.Field(30, 10, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Designation {
            get { return (System.String)this.GetValue("Designation"); }
            set { this.SetValue("Designation", value); }
        }
		
        [BtrieveWrapper.Orm.Field(40, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Salary {
            get { return (System.Boolean)this.GetValue("N_Salary"); }
            set { this.SetValue("N_Salary", value); }
        }
		
        [BtrieveWrapper.Orm.Field(41, 8, BtrieveWrapper.KeyType.Currency, typeof(BtrieveWrapper.Orm.Converters.SignedToDecimalConverter), Parameter = 4, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Salary {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Salary"); }
            set { this.SetValue("Salary", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(49, 25, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Building_Name {
            get { return (System.String)this.GetValue("Building_Name"); }
            set { this.SetValue("Building_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 1)]
        [BtrieveWrapper.Orm.Field(74, 4, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt32Converter))]
        public System.UInt32 Room_Number {
            get { return (System.UInt32)this.GetValue("Room_Number"); }
            set { this.SetValue("Room_Number", value); }
        }
		
        [BtrieveWrapper.Orm.Field(78, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Rsch_Grant_Amount {
            get { return (System.Boolean)this.GetValue("N_Rsch_Grant_Amount"); }
            set { this.SetValue("N_Rsch_Grant_Amount", value); }
        }
		
        [BtrieveWrapper.Orm.Field(79, 8, BtrieveWrapper.KeyType.Float, typeof(BtrieveWrapper.Orm.Converters.DoubleConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Double> Rsch_Grant_Amount {
            get { return (System.Nullable<System.Double>)this.GetValue("Rsch_Grant_Amount"); }
            set { this.SetValue("Rsch_Grant_Amount", value); }
        }

        public static new FacultyKeyCollection Keys { get { return GetKeyCollection<FacultyKeyCollection>(); } }
    }

    public class FacultyKeyCollection : BtrieveWrapper.Orm.KeyCollection<Faculty> {
        public FacultyKeyCollection() : base() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }

        public BtrieveWrapper.Orm.KeyInfo Key2 { get { return this[2]; } }
    }
}