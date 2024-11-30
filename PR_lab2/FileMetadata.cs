using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBProject.Models
{
    public class FileMetadata
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }

        public FileMetadata()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}