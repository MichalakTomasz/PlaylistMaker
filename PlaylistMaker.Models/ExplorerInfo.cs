using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Models
{
    public class ExplorerInfo
    {
        public IEnumerable<(string fullpath, string filemame)> Folders { get; set; }
        public IEnumerable<(string fullpath, string filename)> Files { get; set; }
    }
}
