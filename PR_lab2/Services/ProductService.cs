// Services/ProductService.cs
using MongoDBProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBProject.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new List<Product>();

        public List<Product> GetAll() => _products;

        public Product GetById(string id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            product.Id = Guid.NewGuid().ToString();
            _products.Add(product);
        }

        public void Update(string id, Product updatedProduct)
        {
            var product = GetById(id);
            if (product != null)
            {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                product.Link = updatedProduct.Link;
                product.Currency = updatedProduct.Currency;
                product.Age = updatedProduct.Age;
                product.Pieces = updatedProduct.Pieces;
                product.InsidersPoints = updatedProduct.InsidersPoints;
                product.ItemNumber = updatedProduct.ItemNumber;
                product.Minifigures = updatedProduct.Minifigures;
                product.Dimentions = updatedProduct.Dimentions;
            }
        }

        public void Delete(string id) => _products.RemoveAll(p => p.Id == id);
    }
}
