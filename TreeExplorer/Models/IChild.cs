using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExplorerTreeView
{
    internal interface IChild
    {
        IEnumerable<IBaseNode> Items { get; set; }
    }
}
