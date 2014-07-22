namespace BtrieveWrapper.Orm.Models.CustomModels
{
	public class DemodataDbClient : BtrieveWrapper.Orm.DbClient
    {
        public DemodataDbClient(string dllPath = null, string applicationId = "BW")
            : base(dllPath, applicationId) { }

        public DemodataDbClient(INativeLibrary nativeLibrary, string applicationId = "BW")
            : base(nativeLibrary, applicationId) { }

        public BillingManager Billing(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (BillingManager)this.CreateManager(typeof(BillingManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public ClassManager Class(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (ClassManager)this.CreateManager(typeof(ClassManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public CourseManager Course(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (CourseManager)this.CreateManager(typeof(CourseManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public DeptManager Dept(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (DeptManager)this.CreateManager(typeof(DeptManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public EnrollsManager Enrolls(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (EnrollsManager)this.CreateManager(typeof(EnrollsManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public FacultyManager Faculty(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (FacultyManager)this.CreateManager(typeof(FacultyManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public PersonManager Person(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (PersonManager)this.CreateManager(typeof(PersonManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RoomManager Room(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RoomManager)this.CreateManager(typeof(RoomManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public StudentManager Student(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (StudentManager)this.CreateManager(typeof(StudentManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public TuitionManager Tuition(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (TuitionManager)this.CreateManager(typeof(TuitionManager), path, ownerName, openMode, recycleCount, temporaryBufferId);
        }
    }
}