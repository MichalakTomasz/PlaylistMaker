using PlaylistMaker.Models;

namespace PlaylistMaker.Wrappers
{
    public class TagId3v2Wrapper : TagAudioWrapper<TagAudio>
    {
        public TagId3v2Wrapper(TagAudio tagAudio) => Model = tagAudio;
    }
}
