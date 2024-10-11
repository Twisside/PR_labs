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
        parser.ParseProducts(html);
    }
}