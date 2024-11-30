using MongoDB.Driver;
using MongoDBProject.Models;

namespace MongoDBProject;

public class MongoProductConnector
{
    public string mongoUrl { get; set; }
    public MongoClient client { get; set; }
    public IMongoCollection<Product> productCollection { get; set; }
    public IMongoCollection<FileMetadata> fileMetadataCollection { get; set; }
    public MongoProductConnector(string mongoUrl, string dbName, string productCollectionName, string fileMetadataCollectionName)
    {
        this.client = new MongoClient(mongoUrl);
        var database = client.GetDatabase(dbName);
        this.productCollection = database.GetCollection<Product>(productCollectionName);
        this.fileMetadataCollection = database.GetCollection<FileMetadata>(fileMetadataCollectionName);
    }

    public void WriteSingleProduct(Product product)
    {
        productCollection.InsertOne(product);
    }
    
    public void UpsertProduct(Product product)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        var options = new ReplaceOptions { IsUpsert = true };

        // Perform the upsert
        var result = productCollection.ReplaceOne(filter, product, options);

        if (result.MatchedCount > 0)
        {
            Console.WriteLine("Product updated successfully!");
        }
        else
        {
            Console.WriteLine("Product inserted successfully!");
        }
        
    }
    public void SaveFileMetadata(FileMetadata fileMetadata)
    {
        this.fileMetadataCollection.InsertOne(fileMetadata);
    }
}