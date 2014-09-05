namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(116,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Tuition")]
    public class Tuition : BtrieveWrapper.Orm.Record<Tuition>
    {
        public Tuition() {
            //Initialize record.
        }

		public Tuition(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 4, BtrieveWrapper.KeyType.Autoincrement, typeof(BtrieveWrapper.Orm.Converters.UInt32Converter))]
        public System.UInt32 ID {
            get { return (System.UInt32)this.GetValue("ID"); }
            set { this.SetValue("ID", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(4, 5, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter))]
        public System.String Degree {
            get { return (System.String)this.GetValue("Degree"); }
            set { this.SetValue("Degree", value); }
        }
		
        [BtrieveWrapper.Orm.Field(9, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit1)]
        public System.Boolean Residency {
            get { return (System.Boolean)this.GetValue("Residency"); }
            set { this.SetValue("Residency", value); }
        }
		
        [BtrieveWrapper.Orm.Field(10, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Cost_Per_Credit {
            get { return (System.Boolean)this.GetValue("N_Cost_Per_Credit"); }
            set { this.SetValue("N_Cost_Per_Credit", value); }
        }
		
        [BtrieveWrapper.Orm.Field(11, 4, BtrieveWrapper.KeyType.Float, typeof(BtrieveWrapper.Orm.Converters.FloatConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Single> Cost_Per_Credit {
            get { return (System.Nullable<System.Single>)this.GetValue("Cost_Per_Credit"); }
            set { this.SetValue("Cost_Per_Credit", value); }
        }
		
        [BtrieveWrapper.Orm.Field(15, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Comments {
            get { return (System.Boolean)this.GetValue("N_Comments"); }
            set { this.SetValue("N_Comments", value); }
        }
		
        [BtrieveWrapper.Orm.Field(16, 100, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Comments {
            get { return (System.String)this.GetValue("Comments"); }
            set { this.SetValue("Comments", value); }
        }

        public static new TuitionKeyCollection Keys { get { return GetKeyCollection<TuitionKeyCollection>(); } }
    }

    public class TuitionKeyCollection : BtrieveWrapper.Orm.KeyCollection<Tuition> {
        public TuitionKeyCollection() : base() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }
    }
}