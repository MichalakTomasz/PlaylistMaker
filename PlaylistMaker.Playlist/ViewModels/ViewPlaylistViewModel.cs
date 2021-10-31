using PlaylistMaker.Events;
using PlaylistMaker.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlaylistMaker.Playlist.ViewModels
{
    public class ViewPlaylistViewModel : BindableBase
    {
        public ViewPlaylistViewModel(IEventAggregator eventAggregator)
        {

            eventAggregator.GetEvent<SelectionEvent>().Subscribe(files =>
            {
                var wrappers = files.Select(f => new FileAudioWrapper(f)).ToList();
                Files.Clear();
                Files.AddRange(wrappers);
                eventAggregator.GetEvent<StatusBarEvent>()
                .Publish(new Models.StatusBarInfo { ItemsCount = Files.Count });
            }, ThreadOption.UIThread, true);
        }

        private ObservableCollection<FileAudioWrapper> _files = new ObservableCollection<FileAudioWrapper>();
        
        public ObservableCollection<FileAudioWrapper> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }
        }

        private bool _selectAll;
        public bool SelectAll
        {
            get { return _selectAll; }
            set { SetProperty(ref _selectAll, value); }
        }

        private DelegateCommand _selectAllCommand;
        public DelegateCommand SelectAllCommand =>
            _selectAllCommand ?? (_selectAllCommand = 
            new DelegateCommand(ExecuteSelectAllCommand, CanExecuteSelectAllCommand))
            .ObservesProperty(() => Files);

        private bool CanExecuteSelectAllCommand()
            => Files.Any();

        void ExecuteSelectAllCommand()
        {
            if (SelectAll) Files.ToList().ForEach(f => f.IsSelected = true);
            else Files.ToList().ForEach(f => f.IsSelected = false);
        }
    }
}
