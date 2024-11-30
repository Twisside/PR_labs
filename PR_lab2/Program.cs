using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDBProject.Clients;

namespace MongoDBProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Run the server
            await host.StartAsync();

            // Initialize WebSocket clients after server startup
            string serverUri = "ws://localhost:5000/ws";
            await WebSocketClient.RunClients(serverUri, 4);

            await host.WaitForShutdownAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}



/*
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDBProject.Models;
using System;

namespace MongoDBProject
{
    class Program
    {
        static void Main(string[] args)
        {
            bool debug_mode = false;

            string mongoPath = "mongodb://localhost:27017";
            string dbName = "admin";
            string collectionName = "Products";

            //  leaving the db connection to be first, since no db - no sence to go further
            (MongoClient client, long count) = ConnectMongo(mongoPath, dbName, collectionName);
            MongoProductConnector connector = new MongoProductConnector(mongoPath, dbName, collectionName);

            //      considering that Chrome Driver is fast as Sonic with drugs (well, pretty slow), I've decided
            //  to make this operation running only in case if DB is not populated with product information.
            //  Otherwise, we skip to the interesting part with handling connections and so on
            if (count == 0)
            {
                Console.WriteLine("\n\n DB connected and created, getting base product data from Lego... \n\n");

                //  taking products from the LEGO website
                List<Product> allProducts = new List<Product>();
                List<Product> allProductsDetailed = new List<Product>();
                WebScraper scraper = new WebScraper();

                string url = "https://www.lego.com/bestsellers";
                string html = scraper.GetPageAsync(url).Result;


                HtmlParser parser = new HtmlParser();
                List<List<string>>? everything = parser.ParseElements(html);

                foreach (List<string> item in everything)
                {
                    Product product = new Product();
                    product.Name = item[0];
                    product.Price = item[1];
                    product.Link = item[2];
                    allProducts.Add(product);
                    if (debug_mode)
                    {
                        product.ShowProduct();
                    }
                    Console.WriteLine($"Collected {allProducts.Count} products general overviews...");
                }

                Console.WriteLine("\n\n Getting more detailed products overview ... \n\n");

                foreach (Product item in allProducts)
                {
                    string newLink = "https://www.lego.com/" + item.Link.Substring("/en-us/".Length);
                    List<string> productDetailed = parser.AlternateProductParse(newLink);

                    if (productDetailed.Count == 8)
                    {
                        item.Age = productDetailed[0];
                        item.Pieces = productDetailed[1];
                        item.InsidersPoints = productDetailed[2];
                        item.ItemNumber = productDetailed[3];
                        item.Minifigures = productDetailed[4];

                        List<string> productDimensions = new List<string>();
                        productDimensions.Add(productDetailed[5]);
                        productDimensions.Add(productDetailed[6]);
                        productDimensions.Add(productDetailed[7]);
                        item.Dimentions = productDimensions;

                        allProductsDetailed.Add(item);
                        item.ShowProduct();
                    }
                }

                foreach (Product item in allProductsDetailed)
                {
                    connector.UpsertProduct(item);
                }

            }
            else
            {
                Console.WriteLine("\n\n DB connected and it is already populated with data\n\n");
            }
        }

        static (MongoClient, long) ConnectMongo(string mongoUrl, string dbName, string collectionName,
            bool skipCollectionCheck = false)
        {
            //  make connection DB
            MongoClient client = new MongoClient(mongoUrl);

            //      check if there is a database called "admin". If it is... connect to it, duh
            //      also, had to use lambda conversion of values representing database names from
            //  BSON format to the string one for name check. Because science
            var databases = client.ListDatabases().ToList();
            var databaseNames = databases.Select(
                db => db.GetElement("name").Value.AsString
            ).ToList();
            if (!databaseNames.Contains(dbName))
            {
                throw new ApplicationException(dbName + " database not found");
            }
            var database = client.GetDatabase(dbName);
            Console.WriteLine("Found " + dbName + " database, connection established...");

            //  if flag for this status is true, then no collection check will be done, just connection to mongo
            //  with check of the db presence
            if (skipCollectionCheck)
            {
                return (client, 0);
            }

            //  check if there's a collection Products available. If it's found - show how many elements
            //  it has, else create a collection
            var collections = database.ListCollectionNames().ToList();
            if (!collections.Contains(collectionName))
            {
                database.CreateCollection(collectionName);
                Console.WriteLine("Created the " + collectionName + " collection...");
            }
            else
            {
                Console.WriteLine(collectionName + " collection has been found, connecting...");
            }
            var collection = database.GetCollection<BsonDocument>(collectionName);

            long count = collection.CountDocuments(new BsonDocument());
            Console.WriteLine($"Number of documents in {collectionName} collection: {count}");

            return (client, count);
        }
    }
}
*/
