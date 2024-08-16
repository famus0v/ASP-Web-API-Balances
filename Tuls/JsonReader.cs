using Newtonsoft.Json;

namespace TestApplication.Tuls
{
    public static class JsonReader
    {
        public static List<T> LoadJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return [];

            var jsonData = File.ReadAllText(filePath);
            List<T> deserializedJson = [];
            try
            {
                deserializedJson = JsonConvert.DeserializeObject<List<T>>(jsonData);
            }
            catch (Exception ex)
            {
                try
                {
                    deserializedJson = JsonConvert.DeserializeObject<Dictionary<string, List<T>>>(jsonData)["balance"];
                }
                catch 
                {
                    Console.WriteLine("Error deserializing json: "+ex.Message);
                }
            }
            return deserializedJson;
        }
    }
}