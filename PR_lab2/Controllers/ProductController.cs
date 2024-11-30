using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDBProject.Models;
using MongoDBProject;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly MongoProductConnector _mongoProductConnector;

        public ProductController(MongoProductConnector mongoProductConnector)
        {
            _mongoProductConnector = mongoProductConnector;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var products = await Task.Run(() =>
                _mongoProductConnector.productCollection.Find(_ => true)
                    .Skip(offset)
                    .Limit(limit)
                    .ToList()
            );
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await Task.Run(() => _mongoProductConnector.productCollection.Find(p => p.Id == id).FirstOrDefault());
            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await Task.Run(() => _mongoProductConnector.WriteSingleProduct(product));
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Product updatedProduct)
        {
            var existingProduct = await Task.Run(() => _mongoProductConnector.productCollection.Find(p => p.Id == id).FirstOrDefault());
            if (existingProduct == null)
                return NotFound();

            updatedProduct.Id = id;
            await Task.Run(() => _mongoProductConnector.UpsertProduct(updatedProduct));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await Task.Run(() => _mongoProductConnector.productCollection.DeleteOne(p => p.Id == id));
            if (result.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected");
            }

            var path = Path.Combine("D:\\OneDrive - Technical University of Moldova\\PR\\PR_labs\\PR_lab2\\uploads", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Read the file content
            string fileContent;
            using (var streamReader = new StreamReader(path))
            {
                fileContent = await streamReader.ReadToEndAsync();
            }

            // Deserialize the JSON content to a list of products
            var products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(fileContent);

            // Add or update the products in MongoDB
            foreach (var product in products)
            {
                await Task.Run(() => _mongoProductConnector.UpsertProduct(product));
            }

            return Ok(new
            {
                message = "File uploaded and products updated successfully",
                path
            });
        }


    }
}
