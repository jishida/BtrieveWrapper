namespace BtrieveWrapper.Orm.Models
{
	public class SchemaDbClient : BtrieveWrapper.Orm.DbClient
    {
        public SchemaDbClient(string applicationId = "BW", string dllPath = null, System.Collections.Generic.IEnumerable<string> dependencyPaths = null)
            : base(applicationId, dllPath, dependencyPaths) { }

        public RecordManager<FieldSchema, FieldSchemaKeyCollection> FieldSchema(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000) {
            return (RecordManager<FieldSchema, FieldSchemaKeyCollection>)this.CreateManager<FieldSchema, FieldSchemaKeyCollection>(path, ownerName, openMode, recycleCount);
        }

        public RecordManager<FieldSchema, FieldSchemaKeyCollection> FieldSchema(string path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000) {
            return (RecordManager<FieldSchema, FieldSchemaKeyCollection>)this.CreateManager<FieldSchema, FieldSchemaKeyCollection>(path, ownerName, openMode, recycleCount);
        }

        public RecordManager<FileSchema, FileSchemaKeyCollection> FileSchema(BtrieveWrapper.Orm.Path path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000) {
            return (RecordManager<FileSchema, FileSchemaKeyCollection>)this.CreateManager<FileSchema, FileSchemaKeyCollection>(path, ownerName, openMode, recycleCount);
        }

        public RecordManager<FileSchema, FileSchemaKeyCollection> FileSchema(string path = null, string ownerName = null, BtrieveWrapper.OpenMode? openMode = null, int recycleCount = 1000) {
            return (RecordManager<FileSchema, FileSchemaKeyCollection>)this.CreateManager<FileSchema, FileSchemaKeyCollection>(path, ownerName, openMode, recycleCount);
        }
    }
}