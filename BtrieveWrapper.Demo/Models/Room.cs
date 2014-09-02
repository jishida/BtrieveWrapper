namespace BtrieveWrapper.Orm.Models.CustomModels
{
    [BtrieveWrapper.Orm.Key(0,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Key(1,
        DuplicateKeyOption = BtrieveWrapper.DuplicateKeyOption.RepeatDuplicate,
        IsModifiable = true)]
    [BtrieveWrapper.Orm.Record(54,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Room")]
    public class Room : BtrieveWrapper.Orm.Record<Room>
    {
        public Room() {
            //Initialize record.
        }

		public Room(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(0, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Building_Name {
            get { return (System.Boolean)this.GetValue("N_Building_Name"); }
            set { this.SetValue("N_Building_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 1,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(1, 25, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Building_Name {
            get { return (System.String)this.GetValue("Building_Name"); }
            set { this.SetValue("Building_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 2,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(26, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Number {
            get { return (System.Boolean)this.GetValue("N_Number"); }
            set { this.SetValue("N_Number", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(0, 3)]
        [BtrieveWrapper.Orm.Field(27, 4, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt32Converter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.UInt32> Number {
            get { return (System.Nullable<System.UInt32>)this.GetValue("Number"); }
            set { this.SetValue("Number", value); }
        }
		
        [BtrieveWrapper.Orm.Field(31, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Capacity {
            get { return (System.Boolean)this.GetValue("N_Capacity"); }
            set { this.SetValue("N_Capacity", value); }
        }
		
        [BtrieveWrapper.Orm.Field(32, 2, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt16Converter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.UInt16> Capacity {
            get { return (System.Nullable<System.UInt16>)this.GetValue("Capacity"); }
            set { this.SetValue("Capacity", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(34, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Type {
            get { return (System.String)this.GetValue("Type"); }
            set { this.SetValue("Type", value); }
        }
    }

    public class RoomKeyCollection : BtrieveWrapper.Orm.KeyCollection<Room> {
        public RoomKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }
    }
}