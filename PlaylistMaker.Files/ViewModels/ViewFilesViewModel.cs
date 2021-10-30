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
using System.Collections.ObjectModel;
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
        private readonly IPlayMediaService _playMediaService;
        private CancellationToken _cancelProgress;

        public ViewFilesViewModel(
            IEventAggregator eventAggregator,
            [Dependency(Literals.id3v1)] IID3Service iD3v1Service,
            [Dependency(Literals.id3v2)] IID3Service iD3v2Service,
            IPlayMediaService playMediaService)
        {
            _eventAggregator = eventAggregator;
            _playMediaService = playMediaService;

            DoEvents(eventAggregator, iD3v1Service, iD3v2Service, playMediaService);
            SetPlayerValues();
        }

        private void DoEvents(IEventAggregator eventAggregator, IID3Service iD3v1Service, IID3Service iD3v2Service, IPlayMediaService playMediaService)
        {
            eventAggregator.GetEvent<ExplorerEvent>().Subscribe(e =>
            {
                var fileAudioWrappers = e.Files.Select(f => new FileAudioWrapper(f.fullpath, iD3v1Service, iD3v2Service, playMediaService)).ToList();
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

        private void SetPlayerValues()
        {
            Volume = _playMediaService.Volume;
            Balance = _playMediaService.Balance;
            Position = TimeSpan.FromSeconds(0);
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

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        private IList _selectedItems;
        public IList SelectedItems
        {
            get { return _selectedItems; }
            set { SetProperty(ref _selectedItems, value); }
        }

        private DelegateCommand _selectAllCommand;
        public DelegateCommand SelectAllCommand =>
            _selectAllCommand ?? (_selectAllCommand = 
            new DelegateCommand(ExecuteSelectAllCommand));

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
        
        private DelegateCommand _playCommand;
        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(ExecutePlayCommand, CanExecutePlayCommand));

        void ExecutePlayCommand()
        {
            var se = SelectedItems.Cast<FileAudioWrapper>().ToList();
            var selected = FileAudioWrappers.FirstOrDefault(f => f.IsSelected);
            if (selected != default)
            {
                _playMediaService.Open(selected.FullPath);
                _playMediaService.Play();
                PushProgress(_cancelProgress);
            }
        }

        bool CanExecutePlayCommand()
            => true;

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
    }
}
