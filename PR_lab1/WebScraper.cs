using System.Net.Http;
using System.Threading.Tasks;

class WebScraper
{
    public async Task<string> GetPageAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string htmlContent = await response.Content.ReadAsStringAsync();
                return htmlContent;
            }
            return null;
        }
    }
}