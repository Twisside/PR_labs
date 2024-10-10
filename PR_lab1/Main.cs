namespace PR_lab1;

public class Lab1
{
    public static void Main()
    {
        WebScraper scraper = new();
        
        string url = "https://www.google.com";
        string html = scraper.GetPageAsync(url).Result;
        Console.WriteLine(html);
    }
}