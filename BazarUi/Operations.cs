using BazarUi.Models;
using BazarUi.Utilties;
using System.Text.Json;

namespace BazarUi
{
    public class Operations
    {
        private static readonly LRUCache<string, string> _lruCache = new LRUCache<string, string>();

        public void Search(string topic)
        {
            Console.WriteLine($"Searching for items related to topic: {topic}");
            var path = Environment.GetEnvironmentVariable("CATALOGURL");
            path = path + "/Search/" + topic;
            string jsonResponse = GetJsonResponseFromApi(path, topic);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                List<BookTopicSearchResult> searchResults =
                    JsonUtility.DeserializeSearchResults<BookTopicSearchResult>(jsonResponse);
                DisplaySearchResultsByTopic(searchResults);
            }
            else
            {
                Console.WriteLine("No results found.");
            }
        }

        private string GetJsonResponseFromApi(string path, string topic)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };

            HttpClient httpClient = new HttpClient(socketsHandler);
            var result = httpClient.GetAsync(path).Result;
            return result.Content.ReadAsStringAsync().Result;
        }

        private string GetJsonResponseFromApi(string path)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };

            HttpClient httpClient = new HttpClient(socketsHandler);
            var result = httpClient.GetAsync(path).Result;
            return result.Content.ReadAsStringAsync().Result;
        }

        private void DisplaySearchResultsByTopic(List<BookTopicSearchResult> results)
        {
            if (results.Count > 0)
            {
                Console.WriteLine("Search Results:");
                foreach (var result in results)
                {
                    Console.WriteLine($"Title: {result.title}, Item Price: {result.price}");
                }
            }
            else
            {
                Console.WriteLine("No results found.");
            }
        }

        private void DisplaySearchResults(BookSearchResult result)
        {
            if (result != null)
            {
                Console.WriteLine("Search Results:");

                Console.WriteLine(
                    $"Title: {result.title}\nItem Topic: {result.category}\nItem Price : {result.price}\nStock :{result.stock}\n"
                );
            }
            else
            {
                Console.WriteLine("No results found.");
            }
        }

        public void Info(int itemNumber)
        {
            string cachedData = _lruCache.Get(itemNumber.ToString());

            if (!EqualityComparer<string>.Default.Equals(cachedData, default))
            {
                Console.WriteLine($"Cached Data: {cachedData}");
                BookSearchResult searchResults =
                    JsonUtility.DeserializeSearchResultsForItem<BookSearchResult>(cachedData);
                DisplaySearchResults(searchResults);
            }
            else
            {
                Console.WriteLine($"Fetching info for item number: {itemNumber}");
                Console.WriteLine($"Searching for items related to topic: {itemNumber}");
                var path = Environment.GetEnvironmentVariable("CATALOGURL");

                path = path + "/Book/" + itemNumber;
                Console.WriteLine(path);
                string jsonResponse = String.Empty;
                try
                {
                    jsonResponse = GetJsonResponseFromApi(path);
                }
                catch (Exception error)
                {
                    Console.Error.WriteLine($"Error: {error.Message}");
                }

                Console.WriteLine($"Data fetched from backend and added to the cache:");

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    BookSearchResult searchResults =
                        JsonUtility.DeserializeSearchResultsForItem<BookSearchResult>(jsonResponse);
                    DisplaySearchResults(searchResults);
                    string keyToAdd = itemNumber.ToString();
                    string valueToAdd = jsonResponse;
                    _lruCache.Add(keyToAdd, valueToAdd);
                }
                else
                {
                    Console.WriteLine("No results found.");
                }
            }
        }

        public async void Purchase(int itemNumber)
        {
            var path =
                Environment.GetEnvironmentVariable("ORDERURL") + "/api/Order/Book/" + itemNumber;
            HttpClient client = new HttpClient();
            var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseText = await client.PostAsync(path, content);
            if (responseText.IsSuccessStatusCode)
            {
                var result = responseText.Content.ReadAsStringAsync();
                var bookreturn = JsonUtility.DeserializeSearchResultsForItem<BookSearchResult>(
                    result.Result
                );
                if (bookreturn != null)
                {
                    _lruCache.Update(bookreturn.bookId.ToString(), result.Result);
                    Console.WriteLine($"\n Thank You for Buying {bookreturn.title}");
                }
                else
                {
                    Console.WriteLine($"\n Sorry Book Is Not Avaialble");
                }
            }
            else
            {
                Console.WriteLine($"\n Soory Book is Not avaliable");
            }
        }
    }
}
