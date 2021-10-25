using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Models
{
    public class FileAudio : IFileAudio
    {
        public string Filename { get; set; }
        public string FullPath { get; set; }
        public TagAudio ID3v1Tag { get; set; }
        public TagAudio ID3v2Tag { get; set; }
    }
}
