namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(81,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Course")]
    public class Course : BtrieveWrapper.Orm.Record<Course>
    {
        public Course() {
            //Initialize record.
        }

		public Course(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(0, 7, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Name {
            get { return (System.String)this.GetValue("Name"); }
            set { this.SetValue("Name", value); }
        }
		
        [BtrieveWrapper.Orm.Field(7, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Description {
            get { return (System.Boolean)this.GetValue("N_Description"); }
            set { this.SetValue("N_Description", value); }
        }
		
        [BtrieveWrapper.Orm.Field(8, 50, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Description {
            get { return (System.String)this.GetValue("Description"); }
            set { this.SetValue("Description", value); }
        }
		
        [BtrieveWrapper.Orm.Field(58, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Credit_Hours {
            get { return (System.Boolean)this.GetValue("N_Credit_Hours"); }
            set { this.SetValue("N_Credit_Hours", value); }
        }
		
        [BtrieveWrapper.Orm.Field(59, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.UInt16> Credit_Hours {
            get { return (System.Nullable<System.UInt16>)this.GetValue("Credit_Hours"); }
            set { this.SetValue("Credit_Hours", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(61, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Dept_Name {
            get { return (System.String)this.GetValue("Dept_Name"); }
            set { this.SetValue("Dept_Name", value); }
        }
    }

    public class CourseKeyCollection : BtrieveWrapper.Orm.KeyCollection<Course>
    {
        public CourseKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }
    }
}