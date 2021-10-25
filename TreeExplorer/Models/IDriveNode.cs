using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerTreeView
{
    internal interface IDriveNode :
        IBaseNode,
        IParent,
        IChild,
        IExpandable,
        IPath,
        INotifyPropertyChanged
    {
        string Letter { get; }
    }
}
