using System.Collections.Generic;

namespace PlaylistMaker.Models
{
    public class ExplorerInfo
    {
        public IEnumerable<(string fullpath, string filemame)> Folders { get; set; }
        public IEnumerable<(string fullpath, string filename)> Files { get; set; }
    }
}
