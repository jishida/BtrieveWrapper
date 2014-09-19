namespace BtrieveWrapper.Orm.Tests.Models
{
	public class DemoDbClient : BtrieveWrapper.Orm.DbClient
    {
        public DemoDbClient(string applicationId = "BW", string dllPath = null, System.Collections.Generic.IEnumerable<string> dependencyPaths = null)
            : base(dllPath, applicationId) { }

        public DemoDbClient(INativeLibrary nativeLibrary, string applicationId = "BW")
            : base(nativeLibrary, applicationId) { }

        public RecordManager<Employee, EmployeeKeyCollection> Employee(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Employee, EmployeeKeyCollection>)this.CreateManager<Employee, EmployeeKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Employee, EmployeeKeyCollection> Employee(string path, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Employee, EmployeeKeyCollection>)this.CreateManager<Employee, EmployeeKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Company, CompanyKeyCollection> Company(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Company, CompanyKeyCollection>)this.CreateManager<Company, CompanyKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Company, CompanyKeyCollection> Company(string path, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Company, CompanyKeyCollection>)this.CreateManager<Company, CompanyKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }
    }
}