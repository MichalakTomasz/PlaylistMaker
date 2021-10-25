using System.Collections.Generic;

namespace ExplorerTreeView
{
    interface IExplorerService
        : IFilesVisibility
    {
        void RefreshNode(IBaseNode node);
        IEnumerable<IBaseNode> CreateEmptyDriveNode();
        IEnumerable<IBaseNode> CreateEmptyFolderNode(string path);
        IRootNode RootNode { get; }
        void ExpandPath(string path);
        string FilesFilter { get; set; }
    }
}