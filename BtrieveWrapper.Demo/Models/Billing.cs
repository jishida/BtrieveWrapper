namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(139,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Billing")]
    public class Billing : BtrieveWrapper.Orm.Record<Billing>
    {
        public Billing() {
            //Initialize record.
        }

		public Billing(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 Student_ID {
            get { return (System.UInt64)this.GetValue("Student_ID"); }
            set { this.SetValue("Student_ID", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 1,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(8, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Transaction_Number {
            get { return (System.Boolean)this.GetValue("N_Transaction_Number"); }
            set { this.SetValue("N_Transaction_Number", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 2)]
        [BtrieveWrapper.Orm.Field(9, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.UInt16> Transaction_Number {
            get { return (System.Nullable<System.UInt16>)this.GetValue("Transaction_Number"); }
            set { this.SetValue("Transaction_Number", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 3,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(11, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Log {
            get { return (System.Boolean)this.GetValue("N_Log"); }
            set { this.SetValue("N_Log", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 4)]
        [BtrieveWrapper.Orm.Field(12, 8, BtrieveWrapper.KeyType.Timestamp, typeof(BtrieveWrapper.Orm.Converters.TimestampConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.DateTime> Log {
            get { return (System.Nullable<System.DateTime>)this.GetValue("Log"); }
            set { this.SetValue("Log", value); }
        }
		
        [BtrieveWrapper.Orm.Field(20, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Amount_Owed {
            get { return (System.Boolean)this.GetValue("N_Amount_Owed"); }
            set { this.SetValue("N_Amount_Owed", value); }
        }
		
        [BtrieveWrapper.Orm.Field(21, 4, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), Parameter = 2, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Amount_Owed {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Amount_Owed"); }
            set { this.SetValue("Amount_Owed", value); }
        }
		
        [BtrieveWrapper.Orm.Field(25, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Amount_Paid {
            get { return (System.Boolean)this.GetValue("N_Amount_Paid"); }
            set { this.SetValue("N_Amount_Paid", value); }
        }
		
        [BtrieveWrapper.Orm.Field(26, 4, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), Parameter = 2, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Amount_Paid {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Amount_Paid"); }
            set { this.SetValue("Amount_Paid", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 5)]
        [BtrieveWrapper.Orm.Field(30, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 Registrar_ID {
            get { return (System.UInt64)this.GetValue("Registrar_ID"); }
            set { this.SetValue("Registrar_ID", value); }
        }
		
        [BtrieveWrapper.Orm.Field(38, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Comments {
            get { return (System.Boolean)this.GetValue("N_Comments"); }
            set { this.SetValue("N_Comments", value); }
        }
		
        [BtrieveWrapper.Orm.Field(39, 100, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Comments {
            get { return (System.String)this.GetValue("Comments"); }
            set { this.SetValue("Comments", value); }
        }

        public static new BillingKeyCollection Keys { get { return GetKeyCollection<BillingKeyCollection>(); } }
    }

    public class BillingKeyCollection : BtrieveWrapper.Orm.KeyCollection<Billing> {
        public BillingKeyCollection() : base() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }
    }
}