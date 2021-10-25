using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using Unity;

namespace PlaylistMaker.Wrappers
{
    public class TagId3v1Wrapper : TagAudioWrapper<TagAudio>
    {
        public TagId3v1Wrapper(TagAudio tagAudio) => Model = tagAudio;
    }
}
