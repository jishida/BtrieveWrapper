namespace BtrieveWrapper.Orm.Tests.Models
{
    [BtrieveWrapper.Orm.Key(0)]
    [BtrieveWrapper.Orm.Key(1)]
    [BtrieveWrapper.Orm.Record(64,
        PageSize = 8192,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "localhost",
        UriDbName = "Demo",
        UriTable = "Company")]
    public class Company : BtrieveWrapper.Orm.Record<Company>
    {
        public Company() {
            //Initialize record.
        }

		public Company(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 4, BtrieveWrapper.KeyType.Integer, typeof(BtrieveWrapper.Orm.Converters.Int32Converter))]
        public System.Int32 Id {
            get { return (System.Int32)this.GetValue("Id"); }
            set { this.SetValue("Id", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0)]
        [BtrieveWrapper.Orm.Field(4, 30, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String Name {
            get { return (System.String)this.GetValue("Name"); }
            set { this.SetValue("Name", value); }
        }
		
        [BtrieveWrapper.Orm.Field(34, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Comment {
            get { return (System.Boolean)this.GetValue("N_Comment"); }
            set { this.SetValue("N_Comment", value); }
        }
		
        [BtrieveWrapper.Orm.Field(35, 29, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Comment {
            get { return (System.String)this.GetValue("Comment"); }
            set { this.SetValue("Comment", value); }
        }

        public override bool Equals(object obj) {
            var target = obj as Company;
            if (target == null) {
                return false;
            }
            if (this.Id != target.Id) {
                return false;
            }
            if (this.Name != target.Name) {
                return false;
            }
            if (this.N_Comment != target.N_Comment) {
                return false;
            }
            if (this.Comment != target.Comment) {
                return false;
            }
            return true;
        }

        public override int GetHashCode() {
            return this.Id;
        }

        public static CompanyKeyCollection Keys { get { return GetKeyCollection<CompanyKeyCollection>(); } }
    }

    public class CompanyKeyCollection : BtrieveWrapper.Orm.KeyCollection<Company> {
        public CompanyKeyCollection() { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this[1]; } }
    }
}