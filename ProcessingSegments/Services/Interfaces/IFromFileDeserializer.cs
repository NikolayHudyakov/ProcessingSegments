namespace ProcessingSegments.Services.Interfaces
{
    internal interface IFromFileDeserializer<T>
    {
        public T? Deserialize(string? filePath);
    }
}
