using System.Text;

namespace PR_lab1;
public class ProcessedProducts
{
    public List<Product> FilteredProducts { get; set; }
    public string UTCTimestamp { get; set; }
    
    public string SerializeToJson(ProcessedProducts processedProducts)
    {
        StringBuilder json = new StringBuilder();
        json.Append("{");
        json.Append($"\"UTCTimestamp\": \"{processedProducts.UTCTimestamp}\",");
        json.Append("\"FilteredProducts\": [");

        for (int i = 0; i < processedProducts.FilteredProducts.Count; i++)
        {
            var product = processedProducts.FilteredProducts[i];
            json.Append("{");
            json.Append($"\"Name\": \"{product.Name}\",");
            json.Append($"\"Price\": \"{product.Price} {product.Currency}\",");
            json.Append($"\"Link\": \"{product.Link}\",");
            json.Append($"\"Age\": \"{product.Age}\",");
            json.Append($"\"Pieces\": \"{product.Pieces}\",");
            json.Append($"\"InsidersPoints\": \"{product.InsidersPoints}\",");
            json.Append($"\"ItemNumber\": \"{product.ItemNumber}\",");
            json.Append($"\"Minifugures\": \"{product.Minifigures}\",");
            json.Append($"\"Dimentions\": \"{product.Dimentions[0]} {product.Dimentions[1]} {product.Dimentions[2]}\"");
            
            json.Append("}");

            if (i < processedProducts.FilteredProducts.Count - 1)
            {
                json.Append(",");
            }
        }

        json.Append("]");
        json.Append("}");
        return json.ToString();
    }

    public string SerializeToXml(ProcessedProducts processedProducts)
    {
        StringBuilder xml = new StringBuilder();
        xml.Append($"<ProcessedProducts UTCTimestamp=\"{processedProducts.UTCTimestamp}\">");

        foreach (var product in processedProducts.FilteredProducts)
        {
            xml.Append("<Product>");
            xml.Append($"<Name>{product.Name}</Name>");
            xml.Append($"<Price>{product.Price} $</Price>");
            xml.Append($"<Link>{product.Link}</Link>");
            xml.Append("</Product>");
        }

        xml.Append("</ProcessedProducts>");
        return xml.ToString();
    }

}
