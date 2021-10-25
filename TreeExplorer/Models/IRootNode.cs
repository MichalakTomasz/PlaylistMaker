using System.ComponentModel;

namespace ExplorerTreeView
{
    internal interface IRootNode :
        IBaseNode, 
        IChild, 
        IExpandable,
        INotifyPropertyChanged
    {}
}
