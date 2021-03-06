using PlaylistMaker.Events;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace PlaylistMaker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IExitService _exitService;

        public MainWindowViewModel(IEventAggregator eventAggregator, IExitService exitService)
        {
            _eventAggregator = eventAggregator;
            _exitService = exitService;
            DoEvents();
        }

        private bool _canPlay;
        public bool CanPlay
        {
            get { return _canPlay; }
            set { SetProperty(ref _canPlay, value); }
        }

        private bool _canSave;
        public bool CanSavePlaylist
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        private void DoEvents()
        {
            CanPlayEvent();
            CanSavePlaylistEvent();
        }

        private void CanSavePlaylistEvent()
            => _eventAggregator.GetEvent<CanSaveEvent>()
            .Subscribe(canSave => CanSavePlaylist = canSave);

        private void CanPlayEvent()
            => _eventAggregator.GetEvent<CanPlayEvent>()
            .Subscribe(canPlay => CanPlay = canPlay);

        private DelegateCommand _playCommand;
        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(() =>
            _eventAggregator.GetEvent<MainWindowActionEvent>()
            .Publish(MessageType.Play)));

        private DelegateCommand _pauseCommand;
        public DelegateCommand PauseCommand =>
            _pauseCommand ?? (_pauseCommand = new DelegateCommand(() =>
                _eventAggregator.GetEvent<MainWindowActionEvent>()
            .Publish(MessageType.Pause)));

        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
            _stopCommand ?? (_stopCommand = new DelegateCommand(() =>
            _eventAggregator.GetEvent<MainWindowActionEvent>()
            .Publish(MessageType.Stop)));

        private DelegateCommand _savePlaylistCommand;
        public DelegateCommand SavePlaylistCommand =>
            _savePlaylistCommand ?? (_savePlaylistCommand =
            new DelegateCommand(() => _eventAggregator.GetEvent<MainWindowActionEvent>()
            .Publish(MessageType.SavePlaylist)));

        private DelegateCommand _loadPlaylistCommand;
        public DelegateCommand LoadPlaylistCommand =>
            _loadPlaylistCommand ?? (_loadPlaylistCommand =
            new DelegateCommand(() => _eventAggregator.GetEvent<MainWindowActionEvent>()
            .Publish(MessageType.LoadPlaylist)));

        private DelegateCommand _exitCommand;
        public DelegateCommand ExitCommand =>
            _exitCommand ?? (_exitCommand = new DelegateCommand(() => _exitService.Exit()));

    }
}
