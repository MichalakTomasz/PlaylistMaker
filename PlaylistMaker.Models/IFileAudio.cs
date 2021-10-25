namespace PlaylistMaker.Models
{
    public interface IFileAudio
    {
        string Filename { get; set; }
        string FullPath { get; set; }
        TagAudio ID3v1Tag { get; set; }
        TagAudio ID3v2Tag { get; set; }
    }
}