using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using Unity;

namespace Playlist.Wrappers
{
    public class TagId3v1Wrapper : TagAudioWrapper<TagAudio>
    {
        public TagId3v1Wrapper(string fullPath, [Dependency(Literals.id3v1)]IID3Service iD3Service)
            => Model = iD3Service.GetTag(fullPath);
    }
}
