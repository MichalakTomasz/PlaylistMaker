namespace Playlist.Wrappers
{
    public interface IValidation
    {
        void Validate<T>(T value, string propertyName);
    }
}