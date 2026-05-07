using System.Text.Json;

namespace Project_Final.Utils
{
    public class Util
    {
        public static string WriteObjectToJsonString(object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true});
        }
    }
}
