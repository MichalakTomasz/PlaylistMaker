namespace ExplorerTreeView
{
    static class NodeTextCreator
    {
        #region Methods

        public static string GetText(IBaseNode node)
        {
            switch (node.NodeType)
            {
                case NodeType.Root:
                    return $"Komputer: {UserNameService.LoggedUser}"; ;
                case NodeType.Drive:
                    var letter = (node as IDriveNode).Letter;
                    var label = DriveService.GetLabel(letter);
                    return label.Length == 0 ? $"({letter})":
                        $"{DriveService.GetLabel(letter)} ({letter})";
                case NodeType.Folder:
                    var folderNode = node as IFolderNode;
                    return DirectoriesService.GetFolderName(folderNode.Path);
                case NodeType.File:
                    var fileNode = node as IFileNode;
                    return FileService.GetFileName(fileNode.Path);
                default:
                    return "";
            }
        }

        #endregion//Methods
    }
}
