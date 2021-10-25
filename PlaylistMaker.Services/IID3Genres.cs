using System.Collections.Generic;

namespace PlaylistMaker.Services
{
    public interface IID3Genres
    {
        IReadOnlyList<string> Genres { get; }
    }
}