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
    [BtrieveWrapper.Orm.Record(17,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Enrolls")]
    public class Enrolls : BtrieveWrapper.Orm.Record<Enrolls>
    {
        public Enrolls() {
            //Initialize record.
        }

		public Enrolls(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.KeySegment(2, 0)]
        [BtrieveWrapper.Orm.Field(0, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 Student_ID {
            get { return (System.UInt64)this.GetValue("Student_ID"); }
            set { this.SetValue("Student_ID", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 1)]
        [BtrieveWrapper.Orm.KeySegment(1, 0)]
        [BtrieveWrapper.Orm.Field(8, 4, BtrieveWrapper.KeyType.Integer, typeof(BtrieveWrapper.Orm.Converters.Int32Converter))]
        public System.Int32 Class_ID {
            get { return (System.Int32)this.GetValue("Class_ID"); }
            set { this.SetValue("Class_ID", value); }
        }
		
        [BtrieveWrapper.Orm.Field(12, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Grade {
            get { return (System.Boolean)this.GetValue("N_Grade"); }
            set { this.SetValue("N_Grade", value); }
        }
		
        [BtrieveWrapper.Orm.Field(13, 4, BtrieveWrapper.KeyType.Float, typeof(BtrieveWrapper.Orm.Converters.FloatConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Single> Grade {
            get { return (System.Nullable<System.Single>)this.GetValue("Grade"); }
            set { this.SetValue("Grade", value); }
        }
    }

    public class EnrollsKeyCollection : BtrieveWrapper.Orm.KeyCollection<Enrolls> {
        public EnrollsKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }

        public BtrieveWrapper.Orm.KeyInfo Key2 { get { return this[2]; } }
    }
}