using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExplorerTreeView
{
    class RootNode
        : IRootNode
    {
        #region Constructor

        internal RootNode(IExplorerService explorerService)
        {
            NodeType = NodeType.Root;
            Text = NodeTextCreator.GetText(this);
            ImageName = NodeImageNameCreator.GetName(this);
            Items = explorerService.CreateEmptyDriveNode();
        }

        #endregion//Constructor

        #region Properties

        public NodeType NodeType { get; }
        public IBaseNode Parent { get; }
        public string Text { get; }
        public IEnumerable<IBaseNode> Items { get; set; }
        public string ImageName { get; }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion//Properties

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
