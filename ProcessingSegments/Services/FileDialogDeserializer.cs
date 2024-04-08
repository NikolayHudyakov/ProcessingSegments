using Microsoft.Win32;
using ProcessingSegments.Services.Interfaces;
using System.IO;

namespace ProcessingSegments.Services
{
    public class FileDialogDeserializer<T> : IObjectProviderService<T>
    {
        private const string JsonExt = ".json";

        public T? GetObject()
        {
            OpenFileDialog dialog = new();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string extension = Path.GetExtension(dialog.FileName);

                IFromFileDeserializer<T>? deserializer = extension switch
                {
                    JsonExt => new FromFileJsonDeserializer<T>(),
                    _ => null
                };

                return deserializer != null ? deserializer.Deserialize(dialog.FileName) : default;
            }

            return default;
        }

    }

}
