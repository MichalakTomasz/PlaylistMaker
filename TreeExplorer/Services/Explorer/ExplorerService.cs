using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExplorerTreeView
{
    class ExplorerService
        : IExplorerService
    {
        #region Properties

        public bool ShowFiles { get; set; }
        public string FilesFilter { get; set; }
        public IRootNode RootNode
        {
            get
            {
                if (_rootNode == null)
                    return _rootNode = new RootNode(this);
                return _rootNode;
            }
        }

        #endregion//Properties

        #region Methods

        public IEnumerable<IBaseNode> CreateEmptyDriveNode()
        {
            var list = new ObservableCollection<IDriveNode>();
            if (DrivesService.GetDrivesCount() > 0)
                list.Add(new DriveNode());
            return list;
        }

        public IEnumerable<IBaseNode> CreateEmptyFolderNode(string path)
        {
            var list = new ObservableCollection<IBaseNode>();
            if (DirectoriesService.GetFoldersCount(path) > 0 ||
                (ShowFiles && FileService.GetFilesCount(path) > 0))
                list.Add(new FolderNode());
            return list;
        }

        public void ExpandPath(string path)
        {
            if (DirectoriesService.Exist(path) || FileService.Exist(path))
            {
                var pathArray = path.Split('\\', '/');
                _rootNode.IsExpanded = true;
                IBaseNode tempNode = _rootNode;
                foreach (var item in pathArray)
                {
                    if (tempNode is IChild)
                    {
                        string formatedItem = string.Empty;
                        if (item.Contains(":")) formatedItem = item.Substring(0, 1);
                        else formatedItem = item;
                        if (FindChildNode(tempNode, formatedItem) == null)
                            RefreshNode(tempNode);
                        tempNode = FindChildNode(tempNode, formatedItem);
                        (tempNode as IExpandable).IsExpanded = true;
                    }
                }
            }
        }

        public void RefreshNode(IBaseNode node)
        {
            switch (node.NodeType)
            {
                case NodeType.Root:
                    RefreshRootNode(node);
                    break;
                case NodeType.Drive:
                    RefreshDriveNode(node);
                    break;
                case NodeType.Folder:
                    RefreshFolderNode(node);
                    break;
                case NodeType.File:
                    RefreshFileNode(node);
                    break;
            }
        }

        private void RefreshRootNode(IBaseNode rootNode)
            => InspectDrives(rootNode);

        private void RefreshDriveNode(IBaseNode driveNode)
        {
            if (DriveService.Exist((driveNode as IPath).Path))
            {
                CreateFolderNodes(driveNode);
                if (ShowFiles)
                    CreateFileNodes(driveNode);
            }
            else (((driveNode as IParent)
                    .Parent as IChild)
                    .Items as IList<IBaseNode>)
                    .Remove(driveNode as IBaseNode);
        }

        private void RefreshFolderNode(IBaseNode node)
        {
            var folderNode = node as IParent;
            if (DirectoriesService.Exist((folderNode as IPath).Path))
            {
                CreateFolderNodes(node);
                if (ShowFiles)
                {
                    if (string.IsNullOrWhiteSpace(FilesFilter))
                        CreateFileNodes(node);
                    else CreateFileNodes(node, FilesFilter);
                }
            }
            else ((folderNode.Parent as IChild)
                    .Items as IList<IBaseNode>)
                    .Remove(node as IBaseNode);
        }

        private void RefreshFileNode(IBaseNode node)
        {
            var fileNode = node as IParent;
            if (!FileService.Exist((fileNode as IPath).Path))
                ((fileNode.Parent as IChild)
                    .Items as IList<IBaseNode>)
                    .Remove(fileNode as IBaseNode);
        }

        private void InspectDrives(IBaseNode node)
        {
            var drives = DrivesService.DriveNames as IList<string>;
            var nodeDrives = (node as IChild).Items as IList<IDriveNode>;
            for (int i = 0; i < drives.Count(); i++)
            {
                if (nodeDrives.FirstOrDefault(d => d.Letter == drives[i] &&
                DriveService.GetLabel(d.Letter) == DriveService.GetLabel(drives[i])) == null)
                    (((node as IChild).Items as IList<IDriveNode>)).Insert(i, CreateDriveNode(node, drives[i]));
            }
            if (drives.Count() <  nodeDrives.Count)
            {
                for (int i = 0; i < nodeDrives.Count; i++)
                {
                    if (drives.FirstOrDefault((n => n == nodeDrives[i].Letter &&
                    DriveService.GetLabel(n) == DriveService.GetLabel(nodeDrives[i].Letter))) == null)
                        ((node as IChild).Items as IList<IDriveNode>).Remove(nodeDrives[i]);
                }
            }
        }
        
        private IDriveNode CreateDriveNode(IBaseNode rootNode, string driveLetter)
        {
            return new DriveNode(
                driveLetter,
                this,
                rootNode);
        }

        private void CreateFolderNodes(IBaseNode node)
        {
            IEnumerable<string> folders;
            var driveNode = node as IPath;
            var items = (node as IChild).Items as IList<IBaseNode>;
            if (items.Count > 0) items.Clear();
            folders = DirectoriesService.GetFolders(driveNode.Path);
            foreach (var item in folders)
            {
                items.Add(new FolderNode(
                    item,
                    this,
                    node));
            }
        }

        private void CreateFileNodes(IBaseNode node, string fileFilter = null)
        {
            IEnumerable<string> files;
            var driveNode = node as IPath;
            var items = (node as IChild).Items as IList<IBaseNode>;
            if (string.IsNullOrWhiteSpace(fileFilter))
                files = FileService.GetFiles(driveNode.Path);
            else files = FileService.GetFiles(driveNode.Path, FilesFilter);
            foreach (var item in files)
            {
                items.Add(new FileNode(
                    item,
                    node));
            }
        }
        
        private IBaseNode FindChildNode(IBaseNode node, string subPath)
        {
            var tempNode = node as IChild;
            if (tempNode != null)
            {
                return tempNode.Items.FirstOrDefault(n => (n as IPath).SubPath == subPath);
            }   
            else return null;
        }
        #endregion//Methods

        #region Fields

        private IRootNode _rootNode;

        #endregion//Fields
    }
}
