using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBProject.Clients
{
    public class WebSocketClient
    {
        public static async Task RunClients(string serverUri, int clientCount)
        {
            var clients = new Task[clientCount];

            for (int i = 0; i < clientCount; i++)
            {
                int clientId = i + 1;
                clients[i] = Task.Run(() => StartClient(serverUri, clientId));
            }

            await Task.WhenAll(clients);
        }

        private static async Task StartClient(string serverUri, int clientId)
        {
            using (ClientWebSocket client = new ClientWebSocket())
            {
                await client.ConnectAsync(new Uri(serverUri), CancellationToken.None);
                Console.WriteLine($"Client {clientId} connected.");

                var random = new Random();
                for (int i = 0; i < 10; i++) // Perform 10 operations per client
                {
                    string command = random.Next(2) == 0 ? "read" : "write";
                    if (command == "write")
                    {
                        string randomText = GenerateRandomText();
                        command = $"write {randomText}";
                    }

                    await SendMessageAsync(client, command);
                    await Task.Delay(random.Next(1000, 7000)); // Sleep randomly from 1 to 7 seconds
                }

                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                Console.WriteLine($"Client {clientId} disconnected.");
            }
        }

        private static async Task SendMessageAsync(ClientWebSocket client, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"Sent: {message}");
        }

        private static string GenerateRandomText()
        {
            var random = new Random();
            string[] words = { "Brick", "Set", "Builder", "Creator", "Tower", "Castle", "City", "Space", "Adventure", "Explorer" };
            return $"{words[random.Next(words.Length)]} {words[random.Next(words.Length)]} {words[random.Next(words.Length)]}";
        }
    }
}
