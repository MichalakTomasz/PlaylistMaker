using PlaylistMaker.Commons;
using PlaylistMaker.Events;
using PlaylistMaker.Services;
using PlaylistMaker.Wrappers;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity;

namespace PlaylistMaker.Folders.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        public ViewAViewModel(
            IEventAggregator eventAggregator, 
            [Dependency(Literals.id3v1)]IID3Service iD3v1Service, 
            [Dependency(Literals.id3v2)]IID3Service iD3v2Service)
        {
            eventAggregator.GetEvent<ExplorerEvent>().Subscribe(e =>
            {
                var fileAudioWrappers =  e.Files.Select(f => new FileAudioWrapper(f.fullpath, iD3v1Service, iD3v2Service)).ToList();
                FileAudioWrappers = new ObservableCollection<FileAudioWrapper>(fileAudioWrappers);
            }, ThreadOption.UIThread, true);
        }

        private IEnumerable<string> _filesFullPath;
        public IEnumerable<string> FilesFullPath
        {
            get { return _filesFullPath; }
            set { SetProperty(ref _filesFullPath, value); }
        }

        private ObservableCollection<string> _filesnames = new ObservableCollection<string>();
        public ObservableCollection<string> Filesnames
        {
            get { return _filesnames; }
            set { SetProperty(ref _filesnames, value); }
        }

        private ObservableCollection<FileAudioWrapper> _fileAudioWrappers;
        public ObservableCollection<FileAudioWrapper> FileAudioWrappers
        {
            get { return _fileAudioWrappers; }
            set { SetProperty(ref _fileAudioWrappers, value); }
        }

    }
}
