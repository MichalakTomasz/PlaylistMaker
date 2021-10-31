using PlaylistMaker.Events;
using PlaylistMaker.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistMaker.Explorer.ViewModels
{
    public class ViewExplorerViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ViewExplorerViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        private DelegateCommand _nodeClickCommand;
        public DelegateCommand NodeClickCommand =>
            _nodeClickCommand ?? (_nodeClickCommand = new DelegateCommand(ExecuteNodeClickCommand));

        void ExecuteNodeClickCommand()
        {
            if (FilesFullPath != default && Filesnames != default)
            {
                var explorerInfo = new ExplorerInfo
                {
                    Files = FilesFullPath?.Zip(Filesnames, (fullpath, name) => (fullpath, name))
                };
                _eventAggregator.GetEvent<ExplorerEvent>().Publish(explorerInfo);
            }
            else
            {
                var explorerInfo = new ExplorerInfo
                {
                    Files = Enumerable.Empty<(string, string)>()
                };
                _eventAggregator.GetEvent<ExplorerEvent>().Publish(explorerInfo);
            }
        }

        private IEnumerable<string> _filesFullPath;
        public IEnumerable<string> FilesFullPath
        {
            get { return _filesFullPath; }
            set { SetProperty(ref _filesFullPath, value); }
        }

        private IEnumerable<string> _filesNames;
        public IEnumerable<string> Filesnames
        {
            get { return _filesNames; }
            set { SetProperty(ref _filesNames, value); }
        }

        public string Filter => ".mp3| .mpc| .ogg";
    }
}
