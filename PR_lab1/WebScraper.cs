using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace PR_lab1;

public class WebScraper
{
    public async Task<string> GetPаgeAsync(string url)
    {
        Uri uri = new Uri(url);
        string host = uri.Host;
        int port = 443; // HTTPS uses port 443

        using (TcpClient client = new TcpClient(host, port))
        {
            // Wrap the stream with SslStream for HTTPS
            using (SslStream sslStream = new SslStream(client.GetStream(), false))
            {
                await sslStream.AuthenticateAsClientAsync(host);

                // Send HTTP GET request with User-Agent header
                string request =
                    $"GET {uri.PathAndQuery} HTTP/1.1\r\nHost: {host}\r\nUser-Agent: Mozilla/5.0\r\nConnection: Close\r\n\r\n";
                byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                await sslStream.WriteAsync(requestBytes, 0, requestBytes.Length);

                // Read response
                using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                {
                    string response = await reader.ReadToEndAsync();

                    // Find the body (start after \r\n\r\n)
                    int index = response.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                    if (index != -1)
                    {
                        return response.Substring(index + 4); // Return body content
                    }

                    return null;
                }
            }
        }
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
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