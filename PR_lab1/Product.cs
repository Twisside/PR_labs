namespace PR_lab1;

public class Product
{
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

    public void ShowProduct()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Price: {Price}");
        Console.WriteLine($"Link: {Link}");
        Console.WriteLine($"Currency: $");
        Console.WriteLine($"Age: {Age}");
        Console.WriteLine($"Pieces: {Pieces}");
        Console.WriteLine($"InsidersPoints: {InsidersPoints}");
        Console.WriteLine($"ItemNumber: {ItemNumber}");
        Console.WriteLine($"Minifigures: {Minifigures}");
        foreach (var dim in Dimentions)
        {
            Console.WriteLine($"Dimentions: {Dimentions}");    
        }
        
    }
}