namespace BtrieveWrapper.Orm.Tests.Models
{
    [BtrieveWrapper.Orm.Key(0)]
    [BtrieveWrapper.Orm.Key(1)]
    [BtrieveWrapper.Orm.Record(100,
        PageSize = 8192,
        SystemDataOption = BtrieveWrapper.SystemDataOption.Force,
        UriHost = "localhost",
        UriDbName = "Demo",
        UriTable = "Employee")]
    public class Employee : BtrieveWrapper.Orm.Record<Employee>
    {
        public Employee() {
            //Initialize record.
        }

		public Employee(byte[] dataBuffer) { }
		
        [BtrieveWrapper.Orm.KeySegment(0, 0)]
        [BtrieveWrapper.Orm.Field(0, 4, BtrieveWrapper.KeyType.Integer, typeof(BtrieveWrapper.Orm.Converters.Int32Converter))]
        public System.Int32 Id {
            get { return (System.Int32)this.GetValue("Id"); }
            set { this.SetValue("Id", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 0)]
        [BtrieveWrapper.Orm.Field(4, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String FirstName {
            get { return (System.String)this.GetValue("FirstName"); }
            set { this.SetValue("FirstName", value); }
        }
		
        [BtrieveWrapper.Orm.KeySegment(1, 1)]
        [BtrieveWrapper.Orm.Field(24, 20, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20)]
        public System.String LastName {
            get { return (System.String)this.GetValue("LastName"); }
            set { this.SetValue("LastName", value); }
        }
		
        [BtrieveWrapper.Orm.Field(44, 4, BtrieveWrapper.KeyType.Integer, typeof(BtrieveWrapper.Orm.Converters.Int32Converter))]
        public System.Int32 CompanyId {
            get { return (System.Int32)this.GetValue("CompanyId"); }
            set { this.SetValue("CompanyId", value); }
        }
		
        [BtrieveWrapper.Orm.Field(48, 8, BtrieveWrapper.KeyType.Decimal, typeof(BtrieveWrapper.Orm.Converters.DecimalConverter))]
        public System.Decimal Salary {
            get { return (System.Decimal)this.GetValue("Salary"); }
            set { this.SetValue("Salary", value); }
        }
		
        [BtrieveWrapper.Orm.Field(56, 1, BtrieveWrapper.KeyType.LegacyString, typeof(BtrieveWrapper.Orm.Converters.NullFlagConverter), NullType = BtrieveWrapper.Orm.NullType.NullFlag)]
        public System.Boolean N_Comment {
            get { return (System.Boolean)this.GetValue("N_Comment"); }
            set { this.SetValue("N_Comment", value); }
        }
		
        [BtrieveWrapper.Orm.Field(57, 43, BtrieveWrapper.KeyType.String, typeof(BtrieveWrapper.Orm.Converters.StringConverter), Parameter = 0x20, NullType = BtrieveWrapper.Orm.NullType.Nullable)]
        public System.String Comment {
            get { return (System.String)this.GetValue("Comment"); }
            set { this.SetValue("Comment", value); }
        }

        public override bool Equals(object obj) {
            var target = obj as Employee;
            if (target == null) {
                return false;
            }
            if (this.Id != target.Id) {
                return false;
            }
            if (this.FirstName != target.FirstName) {
                return false;
            }
            if (this.LastName != target.LastName) {
                return false;
            }
            if (this.CompanyId != target.CompanyId) {
                return false;
            }
            if (this.Salary != target.Salary) {
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
    }

    public class EmployeeManager : BtrieveWrapper.Orm.RecordManager<Employee> {
        public EmployeeManager(BtrieveWrapper.Orm.Path path = null, string dllPath = null, string applicationId = "BW", ushort threadId = 0 , string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int reusableCapacity = 1000, byte[] temporaryBuffer = null)
            : base(path, dllPath, applicationId, threadId, ownerName, openMode, reusableCapacity, temporaryBuffer) { }

        public EmployeeManager(BtrieveWrapper.Operator nativeOperator, BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int reusableCapacity = 1000, byte[] temporaryBuffer = null)
            : base(nativeOperator, path, ownerName, openMode, reusableCapacity, temporaryBuffer) { }

        public BtrieveWrapper.Orm.KeyInfo Key0 { get { return this.Keys[0]; } }

        public BtrieveWrapper.Orm.KeyInfo Key1 { get { return this.Keys[1]; } }
    }
}