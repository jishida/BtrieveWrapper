namespace BtrieveWrapper.Orm.Models
{
	public class SchemaDbClient : BtrieveWrapper.Orm.DbClient
    {
        public SchemaDbClient(string dllPath = null, string applicationId = "BW")
            : base(dllPath, applicationId) { }

        public FieldSchemaManager FieldSchema(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000) {
            return (FieldSchemaManager)this.CreateManager(typeof(FieldSchemaManager), path, ownerName, openMode, recycleCount);
        }

        public FileSchemaManager FileSchema(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000) {
            return (FileSchemaManager)this.CreateManager(typeof(FileSchemaManager), path, ownerName, openMode, recycleCount);
        }
    }
}