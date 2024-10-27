using System.Globalization;
using HtmlAgilityPack;

namespace PR_lab1;

public class Lab1
{
    public static void Main()
    {
        List<Product> AllProducts = new List<Product>();
        List<Product> ParsedProd = new List<Product>();
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
            AllProducts.Add(product);
        }
        
        foreach (Product item in AllProducts)
        {
            string rawLink = item.Link;
            string newLink = "https://www.lego.com/" + item.Link.Substring("/en-us/".Length);
            
            /*WebScraper scrap = new WebScraper();
            
            string newHtml = scrap.GetPageAsync(newLink).Result;
            List<List<string>> product = parser.ParseProduct(newHtml);*/
            
            
            List<string> aaaaaa = parser.AlternateProductParse(newLink);

            if (aaaaaa.Count == 8)
            {
                item.Age = aaaaaa[0];
                item.Pieces = aaaaaa[1];
                item.InsidersPoints = aaaaaa[2];
                item.ItemNumber = aaaaaa[3];
                item.Minifigures = aaaaaa[4];
                
                List<string> sdfdsffbxvbcgbvfbsgnsdfb = new List<string>();
                sdfdsffbxvbcgbvfbsgnsdfb.Add(aaaaaa[5]);
                sdfdsffbxvbcgbvfbsgnsdfb.Add(aaaaaa[6]);
                sdfdsffbxvbcgbvfbsgnsdfb.Add(aaaaaa[7]);
                item.Dimentions = sdfdsffbxvbcgbvfbsgnsdfb;
                
                ParsedProd.Add(item);
            }
        }

        foreach (var item in ParsedProd)
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------------------");
            item.ShowProduct();
            //json serialization
            ProcessedProducts processedProducts = new ProcessedProducts();
            processedProducts.FilteredProducts = ParsedProd;
            processedProducts.UTCTimestamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            var json = processedProducts.SerializeToJson(processedProducts);
            Console.WriteLine(json);
            
            //xml serialization
            var xml = processedProducts.SerializeToXml(processedProducts);
            Console.WriteLine(xml);
            
            
            //CustomSerializer
            var customSerializer = new CustomSerializer();
            string serialized = customSerializer.Serialize(item);
            Console.WriteLine(serialized);
            
            
        }
    }
}