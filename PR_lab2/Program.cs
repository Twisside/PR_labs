

using MongoDB.Driver;
using MongoDB.Bson;

var client = new MongoClient("mongodb://localhost/27017");

var database = client.GetDatabase("admin");
database.RenameCollection("Name", "Products");
var collection = database.GetCollection<BsonDocument>("Products");

var indexes = collection.Indexes;

Console.WriteLine($"{database.DatabaseNamespace.DatabaseName} {collection.CollectionNamespace.CollectionName}");
