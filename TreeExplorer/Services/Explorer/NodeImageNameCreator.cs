namespace ExplorerTreeView
{
    static class NodeImageNameCreator
    {
        #region Methods

        public static string GetName(IBaseNode explorerNode)
        {
            switch (explorerNode.NodeType)
            {
                case NodeType.Root:
                    return NodeType.Root.ToString();
                case NodeType.Drive:
                    var letter = (explorerNode as IDriveNode).Letter;
                    return DriveService.GetDriveType(letter).ToString();
                case NodeType.Folder:
                    return explorerNode.NodeType.ToString();
                case NodeType.File:
                    var path = (explorerNode as IFileNode).Path;
                    return path;
                default:
                    return "";
            }
        }

        #endregion//Methods
    }
}
