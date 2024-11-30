using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBProject.Helpers
{
    public static class WebSocketHandler
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private static readonly Mutex FileMutex = new Mutex();
        private static readonly string FilePath = Path.Combine("D:\\OneDrive - Technical University of Moldova\\PR\\PR_labs\\PR_lab2", "interactions_story.txt");
        private static readonly string[] RandomWords = { "Brick", "Set", "Builder", "Creator", "Tower", "Castle", "City", "Space", "Adventure", "Explorer" };

        public static async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            Console.WriteLine("File Path: " + FilePath);
            var socketId = Guid.NewGuid().ToString();
            _sockets.TryAdd(socketId, webSocket);

            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _ = Task.Run(() => ProcessMessage(message, socketId));
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            _sockets.TryRemove(socketId, out _);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private static async Task ProcessMessage(string message, string senderId)
        {
            var random = new Random();
            int delay = random.Next(1000, 7000);
            await Task.Delay(delay);

            FileMutex.WaitOne();
            try
            {
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine("Error: File does not exist at path: " + FilePath);
                    return;
                }

                string responseMessage;
                if (message.StartsWith("read"))
                {
                    string fileContent = await File.ReadAllTextAsync(FilePath);
                    responseMessage = $"[READ] File Length: {fileContent.Length} characters";
                }
                else if (message.StartsWith("write"))
                {
                    string randomText = GenerateRandomText();
                    await File.AppendAllTextAsync(FilePath, $"{randomText}\n");
                    responseMessage = $"[WRITE] Added text: {randomText}";
                }
                else
                {
                    responseMessage = $"[CHAT] {message}";
                }

                await BroadcastMessageAsync(responseMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling file operation: " + ex.Message);
            }
            finally
            {
                FileMutex.ReleaseMutex();
            }
        }

        private static async Task BroadcastMessageAsync(string message)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messageSegment = new ArraySegment<byte>(messageBuffer);

            foreach (var socketPair in _sockets)
            {
                if (socketPair.Value.State == WebSocketState.Open)
                {
                    await socketPair.Value.SendAsync(messageSegment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private static string GenerateRandomText()
        {
            var random = new Random();
            return $"{RandomWords[random.Next(RandomWords.Length)]} {RandomWords[random.Next(RandomWords.Length)]} {RandomWords[random.Next(RandomWords.Length)]}";
        }
    }
}
