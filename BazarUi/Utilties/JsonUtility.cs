using System.Text.Json;

namespace BazarUi.Utilties
{
    public class JsonUtility
    {
        public static List<T> DeserializeSearchResults<T>(string json)
        {
            List<T> results = new List<T>();

            try
            {
                results = JsonSerializer.Deserialize<List<T>>(json);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }

            return results;
        }

        public static T DeserializeSearchResultsForItem<T>(string json)
        {
            T results;

            try
            {
                results = JsonSerializer.Deserialize<T>(json);
                return results;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }

            return default;
        }
    }
}
