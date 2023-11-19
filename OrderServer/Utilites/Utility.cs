using OrderServer.Models;
using System.Text.Json;

namespace OrderServer.Utilites
{
    public class Utility
    {
        internal static async Task<HttpResponseMessage> SendPostApi(string path, Book book)
        {
            book.stock -= 1;
            var data = new { stock = book.stock };
            string jsonString = JsonSerializer.Serialize(data);
            Console.WriteLine(path + jsonString);
            HttpClient client = new HttpClient();
            var content = new StringContent(
                jsonString,
                System.Text.Encoding.UTF8,
                "application/json"
            );
            HttpResponseMessage responseText = await client.PutAsync(path, content);

            return responseText;
        }

        internal static bool CheckIfStockIsVaild(Book book)
        {
            if (book.stock > 0)
            {
                return true;
            }
            return false;
        }

        internal static string FetchApiResponse(string path)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };

            HttpClient httpClient = new HttpClient(socketsHandler);
            var result = httpClient.GetAsync(path).Result;
            return result.Content.ReadAsStringAsync().Result;
        }
    }
}
