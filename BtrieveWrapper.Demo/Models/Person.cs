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
    [BtrieveWrapper.Orm.Record(425,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "127.0.0.1",
        UriDbName = "Demodata",
        UriTable = "Person")]
    public class Person : BtrieveWrapper.Orm.Record<Person>
    {
        public Person() {
            //Initialize record.
        }

		public Person(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 8, BtrieveWrapper.KeyType.UnsignedBinary, typeof(BtrieveWrapper.Orm.Converters.UInt64Converter))]
        public System.UInt64 ID {
            get { return (System.UInt64)this.GetValue("ID"); }
            set { this.SetValue("ID", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 2,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(8, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_First_Name {
            get { return (System.Boolean)this.GetValue("N_First_Name"); }
            set { this.SetValue("N_First_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 3,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(9, 16, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String First_Name {
            get { return (System.String)this.GetValue("First_Name"); }
            set { this.SetValue("First_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(25, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Last_Name {
            get { return (System.Boolean)this.GetValue("N_Last_Name"); }
            set { this.SetValue("N_Last_Name", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 1,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(26, 26, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Last_Name {
            get { return (System.String)this.GetValue("Last_Name"); }
            set { this.SetValue("Last_Name", value); }
        }
		
        [BtrieveWrapper.Orm.Field(52, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Perm_Street {
            get { return (System.Boolean)this.GetValue("N_Perm_Street"); }
            set { this.SetValue("N_Perm_Street", value); }
        }
		
        [BtrieveWrapper.Orm.Field(53, 31, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Perm_Street {
            get { return (System.String)this.GetValue("Perm_Street"); }
            set { this.SetValue("Perm_Street", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 2,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(84, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Perm_City {
            get { return (System.Boolean)this.GetValue("N_Perm_City"); }
            set { this.SetValue("N_Perm_City", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 3,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(85, 31, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Perm_City {
            get { return (System.String)this.GetValue("Perm_City"); }
            set { this.SetValue("Perm_City", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 0,
            IsDescending = true)]
        [BtrieveWrapper.Orm.Field(116, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Perm_State {
            get { return (System.Boolean)this.GetValue("N_Perm_State"); }
            set { this.SetValue("N_Perm_State", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(2, 1,
            IsIgnoreCase = true)]
        [BtrieveWrapper.Orm.Field(117, 3, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Perm_State {
            get { return (System.String)this.GetValue("Perm_State"); }
            set { this.SetValue("Perm_State", value); }
        }
		
        [BtrieveWrapper.Orm.Field(120, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Perm_Zip {
            get { return (System.Boolean)this.GetValue("N_Perm_Zip"); }
            set { this.SetValue("N_Perm_Zip", value); }
        }
		
        [BtrieveWrapper.Orm.Field(121, 11, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Perm_Zip {
            get { return (System.String)this.GetValue("Perm_Zip"); }
            set { this.SetValue("Perm_Zip", value); }
        }
		
        [BtrieveWrapper.Orm.Field(132, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Perm_Country {
            get { return (System.Boolean)this.GetValue("N_Perm_Country"); }
            set { this.SetValue("N_Perm_Country", value); }
        }
		
        [BtrieveWrapper.Orm.Field(133, 21, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Perm_Country {
            get { return (System.String)this.GetValue("Perm_Country"); }
            set { this.SetValue("Perm_Country", value); }
        }
		
        [BtrieveWrapper.Orm.Field(154, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Street {
            get { return (System.Boolean)this.GetValue("N_Street"); }
            set { this.SetValue("N_Street", value); }
        }
		
        [BtrieveWrapper.Orm.Field(155, 31, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Street {
            get { return (System.String)this.GetValue("Street"); }
            set { this.SetValue("Street", value); }
        }
		
        [BtrieveWrapper.Orm.Field(186, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_City {
            get { return (System.Boolean)this.GetValue("N_City"); }
            set { this.SetValue("N_City", value); }
        }
		
        [BtrieveWrapper.Orm.Field(187, 31, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String City {
            get { return (System.String)this.GetValue("City"); }
            set { this.SetValue("City", value); }
        }
		
        [BtrieveWrapper.Orm.Field(218, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_State {
            get { return (System.Boolean)this.GetValue("N_State"); }
            set { this.SetValue("N_State", value); }
        }
		
        [BtrieveWrapper.Orm.Field(219, 3, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String State {
            get { return (System.String)this.GetValue("State"); }
            set { this.SetValue("State", value); }
        }
		
        [BtrieveWrapper.Orm.Field(222, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Zip {
            get { return (System.Boolean)this.GetValue("N_Zip"); }
            set { this.SetValue("N_Zip", value); }
        }
		
        [BtrieveWrapper.Orm.Field(223, 11, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Zip {
            get { return (System.String)this.GetValue("Zip"); }
            set { this.SetValue("Zip", value); }
        }
		
        [BtrieveWrapper.Orm.Field(234, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Phone {
            get { return (System.Boolean)this.GetValue("N_Phone"); }
            set { this.SetValue("N_Phone", value); }
        }
		
        [BtrieveWrapper.Orm.Field(235, 6, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.Decimal> Phone {
            get { return (System.Nullable<System.Decimal>)this.GetValue("Phone"); }
            set { this.SetValue("Phone", value); }
        }
		
        [BtrieveWrapper.Orm.Field(241, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Emergency_Phone {
            get { return (System.Boolean)this.GetValue("N_Emergency_Phone"); }
            set { this.SetValue("N_Emergency_Phone", value); }
        }
		
        [BtrieveWrapper.Orm.Field(242, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Emergency_Phone {
            get { return (System.String)this.GetValue("Emergency_Phone"); }
            set { this.SetValue("Emergency_Phone", value); }
        }
		
        [BtrieveWrapper.Orm.Field(262, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit1)]
        public System.Boolean Unlisted {
            get { return (System.Boolean)this.GetValue("Unlisted"); }
            set { this.SetValue("Unlisted", value); }
        }
		
        [BtrieveWrapper.Orm.Field(263, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Date_Of_Birth {
            get { return (System.Boolean)this.GetValue("N_Date_Of_Birth"); }
            set { this.SetValue("N_Date_Of_Birth", value); }
        }
		
        [BtrieveWrapper.Orm.Field(264, 4, BtrieveWrapper.KeyType.Date, typeof(BtrieveWrapper.Orm.Converters.DateConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.Nullable<System.DateTime> Date_Of_Birth {
            get { return (System.Nullable<System.DateTime>)this.GetValue("Date_Of_Birth"); }
            set { this.SetValue("Date_Of_Birth", value); }
        }
		
        [BtrieveWrapper.Orm.Field(268, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Email_Address {
            get { return (System.Boolean)this.GetValue("N_Email_Address"); }
            set { this.SetValue("N_Email_Address", value); }
        }
		
        [BtrieveWrapper.Orm.Field(269, 31, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Email_Address {
            get { return (System.String)this.GetValue("Email_Address"); }
            set { this.SetValue("Email_Address", value); }
        }
		
        [BtrieveWrapper.Orm.Field(300, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit1)]
        public System.Boolean Sex {
            get { return (System.Boolean)this.GetValue("Sex"); }
            set { this.SetValue("Sex", value); }
        }
		
        [BtrieveWrapper.Orm.Field(301, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Citizenship {
            get { return (System.Boolean)this.GetValue("N_Citizenship"); }
            set { this.SetValue("N_Citizenship", value); }
        }
		
        [BtrieveWrapper.Orm.Field(302, 21, BtrieveWrapper.KeyType.ZString, typeof(BtrieveWrapper.Orm.Converters.ZStringConverter), NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Citizenship {
            get { return (System.String)this.GetValue("Citizenship"); }
            set { this.SetValue("Citizenship", value); }
        }
		
        [BtrieveWrapper.Orm.Field(323, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit1)]
        public System.Boolean Survey {
            get { return (System.Boolean)this.GetValue("Survey"); }
            set { this.SetValue("Survey", value); }
        }
		
        [BtrieveWrapper.Orm.Field(323, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit2)]
        public System.Boolean Smoker {
            get { return (System.Boolean)this.GetValue("Smoker"); }
            set { this.SetValue("Smoker", value); }
        }
		
        [BtrieveWrapper.Orm.Field(323, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit3)]
        public System.Boolean Married {
            get { return (System.Boolean)this.GetValue("Married"); }
            set { this.SetValue("Married", value); }
        }
		
        [BtrieveWrapper.Orm.Field(323, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit4)]
        public System.Boolean Children {
            get { return (System.Boolean)this.GetValue("Children"); }
            set { this.SetValue("Children", value); }
        }
		
        [BtrieveWrapper.Orm.Field(323, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit5)]
        public System.Boolean Disability {
            get { return (System.Boolean)this.GetValue("Disability"); }
            set { this.SetValue("Disability", value); }
        }
		
        [BtrieveWrapper.Orm.Field(323, 1, BtrieveWrapper.KeyType.Bit, typeof(BtrieveWrapper.Orm.Converters.BitBooleanConverter), Parameter = BtrieveWrapper.Orm.Converters.Bit.Bit6)]
        public System.Boolean Scholarship {
            get { return (System.Boolean)this.GetValue("Scholarship"); }
            set { this.SetValue("Scholarship", value); }
        }
		
        [BtrieveWrapper.Orm.Field(324, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Comments {
            get { return (System.Boolean)this.GetValue("N_Comments"); }
            set { this.SetValue("N_Comments", value); }
        }
		
        [BtrieveWrapper.Orm.Field(325, 100, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Comments {
            get { return (System.String)this.GetValue("Comments"); }
            set { this.SetValue("Comments", value); }
        }
    }

    public class PersonKeyCollection : BtrieveWrapper.Orm.KeyCollection<Person>
    {
        public PersonKeyCollection() : base() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }

        public BtrieveWrapper.Orm.KeyInfo Key2 { get { return this[2]; } }
    }
}