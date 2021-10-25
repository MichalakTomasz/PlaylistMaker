using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using Unity;

namespace PlaylistMaker.Wrappers
{
    public class TagId3v2Wrapper : TagAudioWrapper<TagAudio>
    {
        public TagId3v2Wrapper(TagAudio tagAudio) => Model = tagAudio;
    }
}
