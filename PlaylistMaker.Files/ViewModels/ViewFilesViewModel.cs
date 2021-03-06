using PlaylistMaker.Commons;
using PlaylistMaker.Events;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using PlaylistMaker.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace PlaylistMaker.Files.ViewModels
{
    public class ViewFilesViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IID3Service _iD3V1Service;
        private readonly IID3Service _iD3V2Service;
        private readonly IPlayMediaService _playMediaService;
        private CancellationToken _cancelProgress;

        public ViewFilesViewModel(
            IEventAggregator eventAggregator,
            [Dependency(Literals.id3v1)] IID3Service iD3v1Service,
            [Dependency(Literals.id3v2)] IID3Service iD3v2Service,
            IPlayMediaService playMediaService)
        {
            _eventAggregator = eventAggregator;
            _iD3V1Service = iD3v1Service;
            _iD3V2Service = iD3v2Service;
            _playMediaService = playMediaService;

            DoEvents();
            SetPlayerValues();
        }

        private void DoEvents()
        {
            _eventAggregator.GetEvent<ExplorerEvent>().Subscribe(e =>
            {
                var fileAudioWrappers = e.Files.Select(f => new FileAudioWrapper(f.fullpath, _iD3V1Service, _iD3V2Service, _playMediaService)).ToList();
                FileAudioWrappers = fileAudioWrappers;
                SelectedTab = fileAudioWrappers.Any(f => f.ID3v2Tag.HasTag) ? 1 : 0;
                IsAddEnabled = fileAudioWrappers.Any();
                SendMainWindowNotyficacion();
                SelectAll = false;
            }, ThreadOption.UIThread, true);

            _eventAggregator.GetEvent<MainWindowActionEvent>().Subscribe(messageType =>
            {
                var selected = FileAudioWrappers.FirstOrDefault(f => f.IsSelected);
                if (selected != default)
                {
                    switch (messageType)
                    {
                        case MessageType.Play:
                            Play(selected.FullPath);
                            break;
                        case MessageType.Stop:
                            _playMediaService.Stop();
                            break;
                        case MessageType.Pause:
                            _playMediaService.Pause();
                            break;
                    }
                }
            }, ThreadOption.UIThread, true);

            _eventAggregator.GetEvent<StatusBarEvent>()
                .Subscribe(e => IsRemoveEnabled = e.ItemsCount > 0, 
                ThreadOption.UIThread, true);
        }

        private void Play(string path)
        {
            if (!_playMediaService.IsOpened)
                _playMediaService.Open(path);
            _playMediaService.Play();
        }

        private void SendMainWindowNotyficacion()
            => _eventAggregator.GetEvent<CanPlayEvent>()
            .Publish(FileAudioWrappers.Any());

        private void SetPlayerValues()
        {
            Volume = _playMediaService.Volume;
            Balance = _playMediaService.Balance;
            Position = TimeSpan.FromSeconds(0);
        }

        private IEnumerable<FileAudioWrapper> _fileAudioWrappers = new List<FileAudioWrapper>();
        public IEnumerable<FileAudioWrapper> FileAudioWrappers
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

        private TimeSpan _position;
        public TimeSpan Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private double _volume;
        public double Volume
        {
            get { return _volume; }
            set { SetProperty(ref _volume, value); }
        }

        public double Balance
        {
            set
            {
                _playMediaService.Balance = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Balance)));
            }
            get => _playMediaService.Balance;
        }

        private IList _selectedItems;
        public IList SelectedItems
        {
            get { return _selectedItems; }
            set { SetProperty(ref _selectedItems, value); }
        }

        private bool _isAddToPlaylistEnabled;
        public bool IsAddToPlaylistEnabled
        {
            get { return _isAddToPlaylistEnabled; }
            set { SetProperty(ref _isAddToPlaylistEnabled, value); }
        }

        private bool _isCheckEnabled;
        public bool IsCheckEnabled
        {
            get { return _isCheckEnabled; }
            set { SetProperty(ref _isCheckEnabled, value); }
        }

        private bool _isUncheckEnabled;
        public bool IsUncheckEnabled
        {
            get { return _isUncheckEnabled; }
            set { SetProperty(ref _isUncheckEnabled, value); }
        }

        private bool _isPlayEnabled;
        public bool IsPlayEnabled
        {
            get { return _isPlayEnabled; }
            set { SetProperty(ref _isPlayEnabled, value); }
        }

        private bool _isStopEnabled;
        public bool IsStopEnabled
        {
            get { return _isStopEnabled; }
            set { SetProperty(ref _isStopEnabled, value); }
        }

        private bool _isAddEnabled;
        public bool IsAddEnabled
        {
            get { return _isAddEnabled; }
            set { SetProperty(ref _isAddEnabled, value); }
        }

        private bool _isRemoveEnabled;
        public bool IsRemoveEnabled
        {
            get { return _isRemoveEnabled; }
            set { SetProperty(ref _isRemoveEnabled, value); }
        }

        private DelegateCommand _selectAllCommand;
        public DelegateCommand SelectAllCommand =>
            _selectAllCommand ?? (_selectAllCommand = 
            new DelegateCommand(ExecuteSelectAllCommand, () => FileAudioWrappers.Any())
            .ObservesProperty(() => FileAudioWrappers));

        void ExecuteSelectAllCommand()
        {
            if (SelectAll)
                FileAudioWrappers.ToList().ForEach(f => f.IsSelected = true);
            else
                FileAudioWrappers.ToList().ForEach(f => f.IsSelected = false);
        }

        private DelegateCommand _addFilesCommand;
        public DelegateCommand AddFilesCommand =>
            _addFilesCommand ?? (_addFilesCommand = 
            new DelegateCommand(ExecuteAddFilesCommand, CanExecuteAddFilesCommand))
            .ObservesProperty(() => FileAudioWrappers);

        void ExecuteAddFilesCommand()
        {
            SelectedFiles = FileAudioWrappers.Where(f => f.IsSelected).Select(f => f.Model).ToList();
            _eventAggregator.GetEvent<SelectionEvent>().Publish(SelectedFiles);
            FileAudioWrappers.ToList().ForEach(f => f.IsSelected = false);
            SelectAll = false;
        }

        bool CanExecuteAddFilesCommand()
            => true;
        
        private DelegateCommand _playCommand;
        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(() =>
            {
                var selected = SelectedItems.Cast<FileAudioWrapper>().First();
                if (selected != default)
                {
                    Play(selected.FullPath);
                    PushProgress(_cancelProgress);
                }
            }, () => SelectedItems?.Count > 0))
            .ObservesProperty(() => SelectedItems);

        private DelegateCommand _stopCommad;
        public DelegateCommand StopCommand =>
            _stopCommad ?? (_stopCommad = new DelegateCommand(ExecuteStopCommand));

        void ExecuteStopCommand()
        {
            _playMediaService.Stop();
            if (_cancelProgress.CanBeCanceled)
                _cancelProgress.ThrowIfCancellationRequested();
        }

        private void  PushProgress(CancellationToken cancellationToken)
            =>  Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Task.Delay(500);
                    Application.Current.Dispatcher
                    .Invoke(()=> Position = _playMediaService.Position);
                }
            }, cancellationToken);

        private DelegateCommand _volumeCommand;
        public DelegateCommand VolumeCommand =>
            _volumeCommand ?? (_volumeCommand = 
            new DelegateCommand(ExecuteVolumeCommand));

        void ExecuteVolumeCommand()
            => _playMediaService.Volume = Volume;

        private DelegateCommand _pauseCommand;
        public DelegateCommand PauseCommand =>
            _pauseCommand ?? (_pauseCommand = 
            new DelegateCommand(ExecutePauseCommand));

        void ExecutePauseCommand()
            => _playMediaService.Pause();
        private DelegateCommand _removeCommand;
        public DelegateCommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new DelegateCommand(ExecuteRemoveCommand));

        void ExecuteRemoveCommand()
        {
            _eventAggregator.GetEvent<PlaylistEvent>().Publish(
                new PlaylistEventInfo { MessageType = MessageType.Remove });
        }

        private DelegateCommand _addToPlaylistCommand;
        public DelegateCommand AddToPlaylistCommand =>
            _addToPlaylistCommand ?? (_addToPlaylistCommand = 
            new DelegateCommand(ExecuteAddToPlaylistCommand));

        void ExecuteAddToPlaylistCommand()
        {
            _eventAggregator.GetEvent<SelectionEvent>().Publish( SelectedItems?
                .Cast<FileAudioWrapper>().Select(f => f.Model));
        }

        private DelegateCommand _checkItemCommand;
        public DelegateCommand CheckItemCommand =>
            _checkItemCommand ?? (_checkItemCommand = 
            new DelegateCommand(ExecuteCheckItemCommand));

        void ExecuteCheckItemCommand()
        {
            var selected = SelectedItems.Cast<FileAudioWrapper>().FirstOrDefault();
            if (selected != default) selected.IsSelected = true;
        }

        private DelegateCommand _uncheckItemCommand;
        public DelegateCommand UncheckItemCommand =>
            _uncheckItemCommand ?? (_uncheckItemCommand = 
            new DelegateCommand(ExecuteUncheckItemCommand));

        void ExecuteUncheckItemCommand()
        {
            var selected = SelectedItems.Cast<FileAudioWrapper>().FirstOrDefault();
            if (selected != default) selected.IsSelected = false;
        }

        private DelegateCommand _contextMenuOpeningCommand;
        public DelegateCommand ContextMenuOpeningCommand =>
            _contextMenuOpeningCommand ?? (_contextMenuOpeningCommand = new 
            DelegateCommand(ExecuteContextMenuOpeningCommand));

        void ExecuteContextMenuOpeningCommand()
        {
            IsAddToPlaylistEnabled = IsCheckEnabled = IsUncheckEnabled = 
                IsPlayEnabled = IsStopEnabled = FileAudioWrappers.Any();
        }

        private DelegateCommand _selectionChangedCommand;
        public DelegateCommand SelectionChangedCommand =>
            _selectionChangedCommand ?? (_selectionChangedCommand = 
            new DelegateCommand(() => SendMainWindowNotyficacion()));
    }
}
