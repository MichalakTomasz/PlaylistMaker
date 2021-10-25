using System.Collections.Generic;

namespace ExplorerTreeView
{
    interface IDrivesService
    {
        IEnumerable<string> DriveNames { get; }
        int GetDrivesCount();
    }
}
