using System.Collections.Generic;

namespace ExplorerTreeView
{
    class FileNode
        : IFileNode
    {
        #region Constructor

        internal FileNode() { }

        internal FileNode(
            string text,
            IBaseNode parent)
        {
            NodeType = NodeType.File;
            Text = text;
            SubPath = Text;
            Parent = parent;
            Path = PathService.GetPath(this);
            ImageName = NodeImageNameCreator.GetName(this);
        }

        #endregion//Constructor

        #region Properties

        public NodeType NodeType { get; }
        public string Path { get; }
        public string SubPath { get; }
        public string Text { get; }
        public IBaseNode Parent { get; }
        public string ImageName { get; }


        public bool IsExpanded { get; set; } = false;
        public IEnumerable<IBaseNode> Items { get; set; }
        #endregion//Properties
    }
}
