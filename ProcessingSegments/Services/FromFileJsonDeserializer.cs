using Newtonsoft.Json;
using ProcessingSegments.Services.Interfaces;
using System.IO;

namespace ProcessingSegments.Services
{
    public class FromFileJsonDeserializer<T> : IFromFileDeserializer<T>
    {
        public T? Deserialize(string? filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }
    }
}
