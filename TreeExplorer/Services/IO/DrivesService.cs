using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExplorerTreeView
{
    static class DrivesService 
    {
        public static IEnumerable<string> DriveNames
            => DriveInfo.GetDrives()
            .Select(s => s.Name.Substring(0, 1))
            .ToList();

        public static int GetDrivesCount() 
            => DriveInfo.GetDrives().Count();
    }
}
