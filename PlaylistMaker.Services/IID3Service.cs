using PlaylistMaker.Models;
using System.Collections.Generic;

namespace PlaylistMaker.Services
{
    public interface IID3Service
    {
        TagAudio GetTag(string filePath);
        TagVersion GetTagVersion(string filePath);
        bool HasTag(string filePath);
        IReadOnlyList<string> GetGenres();
        void UpdateTag(
            TagAudio audioFile,
            string filePath);
        void RemoveTag(string filePath);
    }
}