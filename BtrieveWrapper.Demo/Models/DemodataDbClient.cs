namespace BtrieveWrapper.Orm.Models.CustomModels
{
	public class DemodataDbClient : BtrieveWrapper.Orm.DbClient
    {
        public DemodataDbClient(string dllPath = null, string applicationId = "BW")
            : base(dllPath, applicationId) { }

        public DemodataDbClient(BtrieveWrapper.INativeLibrary nativeLibrary, string applicationId = "BW")
            : base(nativeLibrary, applicationId) { }

        public RecordManager<Billing, BillingKeyCollection> Billing(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Billing, BillingKeyCollection>)this.CreateManager<Billing, BillingKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Class, ClassKeyCollection> Class(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Class, ClassKeyCollection>)this.CreateManager<Class, ClassKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Course, CourseKeyCollection> Course(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Course, CourseKeyCollection>)this.CreateManager<Course, CourseKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Dept, DeptKeyCollection> Dept(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Dept, DeptKeyCollection>)this.CreateManager<Dept, DeptKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Enrolls, EnrollsKeyCollection> Enrolls(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Enrolls, EnrollsKeyCollection>)this.CreateManager<Enrolls, EnrollsKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Faculty, FacultyKeyCollection> Faculty(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Faculty, FacultyKeyCollection>)this.CreateManager<Faculty, FacultyKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Person, PersonKeyCollection> Person(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Person, PersonKeyCollection>)this.CreateManager<Person, PersonKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Room, RoomKeyCollection> Room(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Room, RoomKeyCollection>)this.CreateManager<Room, RoomKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Student, StudentKeyCollection> Student(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Student, StudentKeyCollection>)this.CreateManager<Student, StudentKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }

        public RecordManager<Tuition, TuitionKeyCollection> Tuition(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000, int temporaryBufferId = 0) {
            return (RecordManager<Tuition, TuitionKeyCollection>)this.CreateManager<Tuition, TuitionKeyCollection>(path, ownerName, openMode, recycleCount, temporaryBufferId);
        }
    }
}