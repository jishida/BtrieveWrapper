namespace BtrieveWrapper.Orm.Tests.Models
{
	public class DemoDbClient : BtrieveWrapper.Orm.DbClient
    {
        public DemoDbClient(string dllPath = null, string applicationId = "BW")
            : base(dllPath, applicationId) { }

        public DemoDbClient(INativeLibrary nativeLibrary, string applicationId = "BW")
            : base(nativeLibrary, applicationId) { }

        public EmployeeManager Employee(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (EmployeeManager)this.CreateManager(typeof(EmployeeManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public CompanyManager Company(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (CompanyManager)this.CreateManager(typeof(CompanyManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }
    }
}