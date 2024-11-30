using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDBProject.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDBProject.Models;
using MongoDBProject.Helpers;

namespace MongoDBProject
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            string mongoPath = "mongodb://localhost:27017";
            string dbName = "admin";
            string productCollectionName = "Products";
            string fileMetadataCollectionName = "FileMetadata";


            services.AddSingleton(new MongoProductConnector(mongoPath, dbName, productCollectionName, fileMetadataCollectionName));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Map("/ws", async context =>
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await WebSocketHandler.HandleWebSocketAsync(webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                });
            });

            // Run preparation commands
            RunPreparationCommands(app);
        }

        private void RunPreparationCommands(IApplicationBuilder app)
        {
            Console.WriteLine("Running preparation commands...");

            string mongoPath = "mongodb://localhost:27017";
            string dbName = "admin";
            string productCollectionName = "Products";
            string fileMetadataCollectionName = "FileMetadata";
            
            bool debug_mode = false;
            (MongoClient client, long count) = ConnectMongo(mongoPath, dbName, productCollectionName);
            MongoProductConnector connector = new MongoProductConnector(mongoPath, dbName, productCollectionName, fileMetadataCollectionName);

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

            Console.WriteLine("Preparation commands completed.");
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
