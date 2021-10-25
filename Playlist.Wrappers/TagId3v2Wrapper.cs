using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using Unity;

namespace Playlist.Wrappers
{
    public class TagId3v2Wrapper : TagAudioWrapper<TagAudio>
    {
        public TagId3v2Wrapper(string fullPath, [Dependency(Literals.id3v2)] IID3Service iD3Service)
            => Model = iD3Service.GetTag(fullPath);

    }
}
