using System;
using System.Collections.Generic;

namespace MongoDBProject.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; } // Assuming price comes as a string like "20 EUR"
        public string Link { get; set; }
        
        public string Currency = "$";
        
        public string Age { get; set; }
        public string Pieces { get; set; }
        public string InsidersPoints { get; set; }
        public string ItemNumber { get; set; }
        public string Minifigures { get; set; }
        public List<string> Dimentions { get; set; }

        // Default constructor
        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }

        // Constructor with random values generation
        public Product(bool generate_random)
        {
            Id = Guid.NewGuid().ToString();
            if (generate_random)
            {
                Name = GenerateRandomName();
                Price = GenerateRandomPrice();
                Link = GenerateRandomLink();
                Age = GenerateRandomAge();
                Pieces = GenerateRandomPieces();
                InsidersPoints = GenerateRandomInsidersPoints();
                ItemNumber = GenerateRandomItemNumber();
                Minifigures = GenerateRandomMinifigures();
                Dimentions = GenerateRandomDimentions();
            }
        }

        private static readonly string[] RandomWords = { "Brick", "Set", "Builder", "Creator", "Tower", "Castle", "City", "Space", "Adventure", "Explorer" };

        private string GenerateRandomName()
        {
            var random = new Random();
            return $"{RandomWords[random.Next(RandomWords.Length)]} {RandomWords[random.Next(RandomWords.Length)]} {RandomWords[random.Next(RandomWords.Length)]}";
        }

        private string GenerateRandomPrice()
        {
            var random = new Random();
            return $"{random.Next(10, 100)} EUR";
        }

        private string GenerateRandomLink()
        {
            var random = new Random();
            return $"https://example.com/product/{random.Next(1000, 9999)}";
        }

        private string GenerateRandomAge()
        {
            var random = new Random();
            return $"{random.Next(3, 18)}+";
        }

        private string GenerateRandomPieces()
        {
            var random = new Random();
            return $"{random.Next(50, 1000)}";
        }

        private string GenerateRandomInsidersPoints()
        {
            var random = new Random();
            return $"{random.Next(10, 500)}";
        }

        private string GenerateRandomItemNumber()
        {
            var random = new Random();
            return $"Item-{random.Next(1000, 9999)}";
        }

        private string GenerateRandomMinifigures()
        {
            var random = new Random();
            return $"{random.Next(1, 10)}";
        }

        private List<string> GenerateRandomDimentions()
        {
            var random = new Random();
            return new List<string>
            {
                $"{random.Next(10, 50)}x{random.Next(10, 50)}x{random.Next(10, 50)} cm"
            };
        }

        public void ShowProduct()
        {
            Console.WriteLine("==========================================");
            if (!string.IsNullOrEmpty(Name))
            {
                Console.WriteLine($"Name: {Name}");
            }

            if (Price != default)
            {
                Console.WriteLine($"Price: {Price}");
            }

            if (!string.IsNullOrEmpty(Link))
            {
                Console.WriteLine($"Link: {Link}");
            }

            Console.WriteLine("Currency: $"); // Assuming the currency is always printed

            if (Age != default)
            {
                Console.WriteLine($"Age: {Age}");
            }

            if (Pieces != default)
            {
                Console.WriteLine($"Pieces: {Pieces}");
            }

            if (InsidersPoints != default)
            {
                Console.WriteLine($"InsidersPoints: {InsidersPoints}");
            }

            if (!string.IsNullOrEmpty(ItemNumber))
            {
                Console.WriteLine($"ItemNumber: {ItemNumber}");
            }

            if (Minifigures != default)
            {
                Console.WriteLine($"Minifigures: {Minifigures}");
            }

            if (Dimentions != null && Dimentions.Any())
            {
                foreach (var dim in Dimentions)
                {
                    Console.WriteLine($"Dimentions: {dim}");
                }
            }
            Console.WriteLine("===================================");
        }
    }
}
