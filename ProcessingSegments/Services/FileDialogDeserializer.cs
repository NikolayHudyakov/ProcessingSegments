using Microsoft.Win32;

namespace ProcessingSegments.Services
{
    public class FileDialogDeserializer<T> : JsonDeserializer<T>
    {
        protected override string? FileName => GetFileName();

        private string? GetFileName()
        {
            OpenFileDialog dialog = new();
            bool? result = dialog.ShowDialog();

            return result == true ? dialog.FileName : null;
        }
    }
}
