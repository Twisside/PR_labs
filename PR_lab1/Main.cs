using System.Globalization;
using HtmlAgilityPack;

namespace PR_lab1;

public class Lab1
{
    public static void Main()
    {
        WebScraper scraper = new WebScraper();
        
        string url = "https://www.lego.com/bestsellers";
        string html = scraper.GetPageAsync(url).Result;

        
        HtmlParser parser = new HtmlParser();
        List<List<string>> everything = parser.ParseElements(html);
        
        foreach (List<string> item in everything)
        {
            foreach (var element in item)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }
        
        foreach (List<string> item in everything)
        {
            string rawLink = item[2];
            string newLink = "https://www.lego.com/" + item[2].Substring("/en-us/".Length);
            
            WebScraper scrap = new WebScraper();
            
            string newHtml = scrap.GetPageAsync(newLink).Result;
            List<List<string>> product = parser.ParseProduct(newHtml);
            
            
            foreach (List<string> fbghasu in product)
            {
                foreach (var element in fbghasu)
                {
                    Console.Write(element + " ");
                }
                Console.WriteLine();
            }
        }
    }
}