using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerTreeView
{
    internal interface IFolderNode :
        IBaseNode,
        IPath,
        IParent,
        IChild,
        IExpandable,
        INotifyPropertyChanged
    {}
}
