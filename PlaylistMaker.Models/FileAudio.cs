using System;

namespace PlaylistMaker.Models
{
    public class FileAudio : IFileAudio
    {
        public string Filename { get; set; }
        public string FullPath { get; set; }
        public TimeSpan Length { get; set; }
        public TagAudio ID3v1Tag { get; set; }
        public TagAudio ID3v2Tag { get; set; }
    }
}
