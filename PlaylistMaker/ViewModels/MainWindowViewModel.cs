using PlaylistMaker.Commons;
using PlaylistMaker.Events;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace PlaylistMaker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private DelegateCommand _playCommand;
        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(ExecutePlayCommand));

        void ExecutePlayCommand()
            => _eventAggregator.GetEvent<MainWindowEvent>().Publish(new MainWindowInfo { MessageType = MessageType.Play });

        private DelegateCommand _pauseCommand;
        public DelegateCommand PauseCommand =>
            _pauseCommand ?? (_pauseCommand = new DelegateCommand(ExecutePauseCommand));

        void ExecutePauseCommand()
            => _eventAggregator.GetEvent<MainWindowEvent>().Publish(new MainWindowInfo { MessageType = MessageType.Pause });

        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
            _stopCommand ?? (_stopCommand = new DelegateCommand(ExecuteStopCommand));

        void ExecuteStopCommand()
            => _eventAggregator.GetEvent<MainWindowEvent>().Publish(new MainWindowInfo { MessageType = MessageType.Stop });

        private DelegateCommand _savePlaylistCommand;
        public DelegateCommand SavePlaylistCommand =>
            _savePlaylistCommand ?? (_savePlaylistCommand =
            new DelegateCommand(() => _eventAggregator.GetEvent<MainWindowEvent>()
            .Publish(new MainWindowInfo { MessageType = MessageType.SavePlaylist })));
    }

}
