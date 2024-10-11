namespace PR_lab1;

using HtmlAgilityPack;

class HtmlParser
{
    public void ParseProducts(string htmlContent)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Assuming products are within <div class="product-item">
        var products = doc.DocumentNode.SelectNodes("//ul[@id='product-listing-grid']/li");
        if (products == null)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            // Fixing relative XPath for name
            var nameNode = product.SelectSingleNode(".//h3/a/span");
            string name = nameNode != null ? nameNode.InnerText.Trim() : "Name not found";

            // Fixing price extraction with null check
            var priceNode = product.SelectSingleNode(".//div[@class='ProductLeaf_priceRow__RUx3P']/span");
            string price = priceNode != null ? priceNode.InnerText.Trim() : "Price not found";

            // Displaying name and price
            if(!name.Contains("not found") && !price.Contains("not found"))
                Console.WriteLine($"{name}, {price}");
        }
    }
}
