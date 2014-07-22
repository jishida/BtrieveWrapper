namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(2,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(3,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(66,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Class")]
    public class Class : BtrieveWrapper.Orm.Record<Class>
    {
        public Class() {
            //Initialize record.
        }

		public Class(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 4, BtrieveWrapper.KeyType.Autoincrement, typeof(BtrieveWrapper.Orm.Converters.UInt32Converter))]
        public System.UInt32 ID {
            get { return (System.UInt32)this.GetValue("ID"); }
            set { this.SetValue("ID", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(4, 7, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Name {
            get { return (System.String)this.GetValue("Name"); }
            set { this.SetValue("Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 1,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(11, 3, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Section {
            get { return (System.String)this.GetValue("Section"); }
            set { this.SetValue("Section", value); }
        }
		
        [BtrieveWrapper.Orm.Field(14, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Max_Size {
            get { return (System.Boolean)this.GetValue("N_Max_Size"); }
            set { this.SetValue("N_Max_Size", value); }
        }
		
        [BtrieveWrapper.Orm.Field(15, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.UInt16> Max_Size {
            get { return (System.Nullable<System.UInt16>)this.GetValue("Max_Size"); }
            set { this.SetValue("Max_Size", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 1)]
        [BtrieveWrapper.Orm.KeySegment(3, 2)]
        [BtrieveWrapper.Orm.Field(17, 4, BtrieveWrapper.KeyType.Date, typeof(BtrieveWrapper.Orm.Converters.DateConverter))]
        public System.DateTime Start_Date {
            get { return (System.DateTime)this.GetValue("Start_Date"); }
            set { this.SetValue("Start_Date", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 2)]
        [BtrieveWrapper.Orm.KeySegment(3, 3)]
        [BtrieveWrapper.Orm.Field(21, 4, BtrieveWrapper.KeyType.Time, typeof(BtrieveWrapper.Orm.Converters.TimeConverter))]
        public BtrieveWrapper.Orm.Time Start_Time {
            get { return (BtrieveWrapper.Orm.Time)this.GetValue("Start_Time"); }
            set { this.SetValue("Start_Time", value); }
        }
		
        [BtrieveWrapper.Orm.Field(25, 4, BtrieveWrapper.KeyType.Time, typeof(BtrieveWrapper.Orm.Converters.TimeConverter))]
        public BtrieveWrapper.Orm.Time Finish_Time {
            get { return (BtrieveWrapper.Orm.Time)this.GetValue("Finish_Time"); }
            set { this.SetValue("Finish_Time", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(3, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(29, 25, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Building_Name {
            get { return (System.String)this.GetValue("Building_Name"); }
            set { this.SetValue("Building_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(3, 1)]
        [BtrieveWrapper.Orm.Field(54, 4, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt32Converter))]
        public System.UInt32 Room_Number {
            get { return (System.UInt32)this.GetValue("Room_Number"); }
            set { this.SetValue("Room_Number", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 0)]
        [BtrieveWrapper.Orm.Field(58, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 Faculty_ID {
            get { return (System.UInt64)this.GetValue("Faculty_ID"); }
            set { this.SetValue("Faculty_ID", value); }
        }
    }

    public class ClassManager : BtrieveWrapper.Orm.RecordManager<Class> {
        public ClassManager(BtrieveWrapper.Orm.Path path = null, string dllPath = null, string applicationId = "BW", ushort threadId = 0 , string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int reusableCapacity = 1000, byte[] temporaryBuffer = null)
            : base(path, dllPath, applicationId, threadId, ownerName, openMode, reusableCapacity, temporaryBuffer) { }

        public ClassManager(BtrieveWrapper.Operator nativeOperator, BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int reusableCapacity = 1000, byte[] temporaryBuffer = null)
            : base(nativeOperator, path, ownerName, openMode, reusableCapacity, temporaryBuffer) { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this.Keys[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this.Keys[1]; } }

        public BtrieveWrapper.Orm.KeyInfo Key2 { get { return this.Keys[2]; } }

        public BtrieveWrapper.Orm.KeyInfo Key3 { get { return this.Keys[3]; } }
    }
}