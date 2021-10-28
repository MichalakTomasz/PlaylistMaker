using PlaylistMaker.Commons;
using PlaylistMaker.Events;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using PlaylistMaker.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity;

namespace PlaylistMaker.Files.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayMediaService _playMediaService;

        public ViewAViewModel(
            IEventAggregator eventAggregator, 
            [Dependency(Literals.id3v1)]IID3Service iD3v1Service, 
            [Dependency(Literals.id3v2)]IID3Service iD3v2Service,
            IPlayMediaService playMediaService)
        {
            _eventAggregator = eventAggregator;
            _playMediaService = playMediaService;

            eventAggregator.GetEvent<ExplorerEvent>().Subscribe(e =>
            {
                var fileAudioWrappers =  e.Files.Select(f => new FileAudioWrapper(f.fullpath, iD3v1Service, iD3v2Service, playMediaService)).ToList();
                FileAudioWrappers = new ObservableCollection<FileAudioWrapper>(fileAudioWrappers);
                IsSelectAllEnabled = fileAudioWrappers.Any();
                SelectedTab = fileAudioWrappers.Any(f => f.ID3v2Tag.HasTag) ? 1 : 0;
            }, ThreadOption.UIThread, true);

            eventAggregator.GetEvent<MainWindowEvent>().Subscribe(e =>
            {
                var selected = FileAudioWrappers.FirstOrDefault(f => f.IsSelected);
                if (selected != default)
                {
                    switch (e.MessageType)
                    { 
                        case MessageType.Play:
                        _playMediaService.Open(selected.FullPath);
                        _playMediaService.Play();
                        break;
                        case MessageType.Stop:
                            _playMediaService.Stop();
                            break;
                        case MessageType.Pause:
                            _playMediaService.Stop();
                            break;
                    }
                }
            }, ThreadOption.UIThread, true);
        }

        private ObservableCollection<FileAudioWrapper> _fileAudioWrappers;
        public ObservableCollection<FileAudioWrapper> FileAudioWrappers
        {
            get { return _fileAudioWrappers; }
            set { SetProperty(ref _fileAudioWrappers, value); }
        }
        
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set { SetProperty(ref _selectedTab, value); }
        }

        private IEnumerable<FileAudio> _selectedFiles = new List<FileAudio>();
        public IEnumerable<FileAudio> SelectedFiles
        {
            get { return _selectedFiles; }
            set { SetProperty(ref _selectedFiles, value); }
        }

        private bool _selectAll;
        public bool SelectAll
        {
            get { return _selectAll; }
            set { SetProperty(ref _selectAll, value); }
        }

        private bool _isSelectAllEnabled;
        public bool IsSelectAllEnabled
        {
            get { return _isSelectAllEnabled; }
            set { SetProperty(ref _isSelectAllEnabled, value); }
        }

        private DelegateCommand _selectAllCommand;
        public DelegateCommand SelectAllCommand =>
            _selectAllCommand ?? (_selectAllCommand = new DelegateCommand(ExecuteSelectAllCommand));

        void ExecuteSelectAllCommand()
        {
            if (SelectAll)
                FileAudioWrappers.ToList().ForEach(f => f.IsSelected = true);
            else
                FileAudioWrappers.ToList().ForEach(f => f.IsSelected = false);
        }

        private DelegateCommand _addFilesCommand;
        public DelegateCommand AddFilesCommand =>
            _addFilesCommand ?? (_addFilesCommand = new DelegateCommand(ExecuteAddFilesCommand, CanExecuteAddFilesCommand))
            .ObservesProperty(() => FileAudioWrappers);

        void ExecuteAddFilesCommand()
        {
            SelectedFiles = FileAudioWrappers.Where(f => f.IsSelected).Select(f => f.Model).ToList();
            _eventAggregator.GetEvent<SelectionEvent>().Publish(SelectedFiles);
        }

        bool CanExecuteAddFilesCommand()
            => true;
    }
}
