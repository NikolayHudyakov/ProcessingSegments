namespace ProcessingSegments.Services.Interfaces
{
    public interface IObjectProviderService<T>
    {
        public T? GetObject();
    }
}
