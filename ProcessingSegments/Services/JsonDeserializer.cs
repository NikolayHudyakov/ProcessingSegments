using Newtonsoft.Json;
using ProcessingSegments.Services.Interfaces;
using System.IO;

namespace ProcessingSegments.Services
{
    public abstract class JsonDeserializer<T> : IObjectProviderService<T>
    {
        protected abstract string? FileName { get; }

        public T? GetObject()
        {
            string? fileName = FileName;

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }
    }
}
