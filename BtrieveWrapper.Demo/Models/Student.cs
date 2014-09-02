namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(76,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Student")]
    public class Student : BtrieveWrapper.Orm.Record<Student>
    {
        public Student() {
            //Initialize record.
        }

		public Student(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 ID {
            get { return (System.UInt64)this.GetValue("ID"); }
            set { this.SetValue("ID", value); }
        }
		
        [BtrieveWrapper.Orm.Field(8, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Cumulative_GPA {
            get { return (System.Boolean)this.GetValue("N_Cumulative_GPA"); }
            set { this.SetValue("N_Cumulative_GPA", value); }
        }
		
        [BtrieveWrapper.Orm.Field(9, 3, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), Parameter = 3, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Cumulative_GPA {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Cumulative_GPA"); }
            set { this.SetValue("Cumulative_GPA", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0)]
        [BtrieveWrapper.Orm.Field(12, 4, BtrieveWrapper.KeyType.Integer, typeof(BtrieveWrapper.Orm.Converters.Int32Converter))]
        public System.Int32 Tuition_ID {
            get { return (System.Int32)this.GetValue("Tuition_ID"); }
            set { this.SetValue("Tuition_ID", value); }
        }
		
        [BtrieveWrapper.Orm.Field(16, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Transfer_Credits {
            get { return (System.Boolean)this.GetValue("N_Transfer_Credits"); }
            set { this.SetValue("N_Transfer_Credits", value); }
        }
		
        [BtrieveWrapper.Orm.Field(17, 3, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Transfer_Credits {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Transfer_Credits"); }
            set { this.SetValue("Transfer_Credits", value); }
        }
		
        [BtrieveWrapper.Orm.Field(20, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Major {
            get { return (System.Boolean)this.GetValue("N_Major"); }
            set { this.SetValue("N_Major", value); }
        }
		
        [BtrieveWrapper.Orm.Field(21, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Major {
            get { return (System.String)this.GetValue("Major"); }
            set { this.SetValue("Major", value); }
        }
		
        [BtrieveWrapper.Orm.Field(41, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Minor {
            get { return (System.Boolean)this.GetValue("N_Minor"); }
            set { this.SetValue("N_Minor", value); }
        }
		
        [BtrieveWrapper.Orm.Field(42, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Minor {
            get { return (System.String)this.GetValue("Minor"); }
            set { this.SetValue("Minor", value); }
        }
		
        [BtrieveWrapper.Orm.Field(62, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Scholarship_Amount {
            get { return (System.Boolean)this.GetValue("N_Scholarship_Amount"); }
            set { this.SetValue("N_Scholarship_Amount", value); }
        }
		
        [BtrieveWrapper.Orm.Field(63, 10, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), Parameter = 2, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Scholarship_Amount {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Scholarship_Amount"); }
            set { this.SetValue("Scholarship_Amount", value); }
        }
		
        [BtrieveWrapper.Orm.Field(73, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Cumulative_Hours {
            get { return (System.Boolean)this.GetValue("N_Cumulative_Hours"); }
            set { this.SetValue("N_Cumulative_Hours", value); }
        }
		
        [BtrieveWrapper.Orm.Field(74, 2, BtrieveWrapper.KeyType.Integer, typeof(BtrieveWrapper.Orm.Converters.Int16Converter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Int16> Cumulative_Hours {
            get { return (System.Nullable<System.Int16>)this.GetValue("Cumulative_Hours"); }
            set { this.SetValue("Cumulative_Hours", value); }
        }
    }

    public class StudentKeyCollection : BtrieveWrapper.Orm.KeyCollection<Student>
    {
        public StudentKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }
    }
}