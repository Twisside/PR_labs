using System.Text.RegularExpressions;

namespace MongoDBProject;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;

class HtmlParser
{
    public List<List<string>>? ParseElements(string htmlContent)
    {
        List<List<string>>? returnlist = new List<List<string>>();
        
        
        string link = "";
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Assuming products are within <div class="product-item">
        var products = doc.DocumentNode.SelectNodes("//ul[@id='product-listing-grid']/li");
        if (products == null)
        {
            Console.WriteLine("No products found.");
            return null;
        }

        foreach (var product in products)
        {
            List<string> productInfo = new List<string>();
            // Fixing relative XPath for name
            var nameNode = product.SelectSingleNode(".//h3/a/span");
            string name = nameNode != null ? nameNode.InnerText.Trim() : "Name not found";

            // Fixing price extraction with null check
            var priceNode = product.SelectSingleNode(".//div[@class='ProductLeaf_priceRow__RUx3P']/span");
            string price = priceNode != null ? priceNode.InnerText.Trim() : "Price not found";
            
            var linkNode = product.SelectSingleNode(".//a[@data-test='product-leaf-title']");
            link = linkNode != null ? linkNode.GetAttributeValue("href", string.Empty) : "Link not found";

            // Displaying name and price
            if (!name.Contains("not found") && !price.Contains("not found"))
            {
                productInfo.Add(name);
                productInfo.Add(price);
                productInfo.Add(link);
                returnlist.Add(productInfo);
            }
        }
        return returnlist;
    }
    
    /*public List<List<string>> ParseProduct(string htmlContent)
    {
        List<List<string>> returnlist = new List<List<string>>();
    
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Locate all product nodes (adjust XPath as needed)
        var statsNodes = doc.DocumentNode.SelectNodes("//section/div[@data-test='product-attributes']/div");
        /*if (statsNodes == null)
        {
            Console.WriteLine("No stats found.");
            return returnlist;
        }#1#

        foreach (var item in statsNodes)
        {
            List<string> productInfo = new List<string>();

            // Adjusted XPath to find "pieces"
            var piecesNode =
                item.SelectSingleNode(
                    ".//div[@data-test='pieces-value']/span/span");
            /*SelectSingleNode(".//span[@data-test='pieces-value']");#1#
            string pieces = piecesNode != null ? piecesNode.InnerText.Trim() : "Pieces not found";

            // Check if pieces information was found and add it to the list
            if (!pieces.Contains("not found"))
            {
                productInfo.Add(pieces);
                returnlist.Add(productInfo);
            }
            else
            {
                Console.WriteLine("Pieces not found.");
            }
        
        }
        return returnlist;
    }*/
    
    public List<string> AlternateProductParse(string link)
    {
        List<string> spanTexts = new List<string>();
        // Initialize the ChromeDriver
        var options = new ChromeOptions();
        options.AddArgument("headless"); // Optional, run in headless mode
        IWebDriver driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl(link);

        var productAttributesDiv = driver.FindElement(By.XPath("//div[@data-test='product-attributes']"));

        if (productAttributesDiv != null)
        {
            /*Console.WriteLine("Found the div with data-test='product-attributes'");*/
            
            
            var outerDivs = productAttributesDiv.FindElements(By.XPath(".//div/div/span"));

            foreach (var span in outerDivs)
            {
                if (span.Text.ToCharArray().Any(char.IsDigit))
                    spanTexts.Add(span.Text);
            }

            // Print the collected texts
            foreach (var text in spanTexts)
            {
                Console.WriteLine(text);
            }
        }
        else
        {
            Console.WriteLine("Div with data-test='product-attributes' not found.");
        }

        // Close the browser
        driver.Quit();
        return spanTexts;
    }

}
