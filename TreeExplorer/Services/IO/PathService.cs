using System.Text;

namespace ExplorerTreeView
{
    static class PathService
    {
        public static string GetPath(IPath node)
        {
            var tempNode = node;
            var sbuilder = new StringBuilder();
            while (tempNode != null)
            {
                switch ((tempNode as IBaseNode).NodeType)
                {
                    case NodeType.Drive:
                        sbuilder.Insert(0,$@"{(tempNode as IDriveNode).Letter}:\");
                        break;
                    case NodeType.Folder:
                        sbuilder.Insert(0, $@"{(tempNode as IFolderNode).Text}\");
                        break;
                    case NodeType.File:
                        sbuilder.Append($@"{(tempNode as IFileNode).Text}");
                        break;
                }
                tempNode = (tempNode as IParent).Parent as IPath;
            }
            return sbuilder.ToString();
        }
    }
}
