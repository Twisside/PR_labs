namespace PR_lab1;

using HtmlAgilityPack;

class HtmlParser
{
    public List<List<string>> ParseElements(string htmlContent)
    {
        List<List<string>> returnlist = new List<List<string>>();
        
        
        string link = "";
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Assuming products are within <div class="product-item">
        var products = doc.DocumentNode.SelectNodes("//ul[@id='product-listing-grid']/li");
        if (products == null)
        {
            Console.WriteLine("No products found.");
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
    
    public List<List<string>> ParseProduct(string htmlContent)
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
        }*/

        foreach (var item in statsNodes)
        {
            List<string> productInfo = new List<string>();

            // Adjusted XPath to find "pieces"
            var piecesNode =
                item.SelectSingleNode(
                    ".//div[@data-test='pieces-value']/span/span");
            /*SelectSingleNode(".//span[@data-test='pieces-value']");*/
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
    }


    
}
