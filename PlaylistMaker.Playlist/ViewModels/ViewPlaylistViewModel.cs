using PlaylistMaker.Events;
using PlaylistMaker.Models;
using PlaylistMaker.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlaylistMaker.Playlist.ViewModels
{
    public class ViewPlaylistViewModel : BindableBase
    {
        public ViewPlaylistViewModel(IEventAggregator eventAggregator)
        {
            DoEvents(eventAggregator);
        }

        private void DoEvents(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<SelectionEvent>().Subscribe(files =>
            {
                var wrappers = files.Select(f => new FileAudioWrapper(f)).ToList();
                Files = new List<FileAudioWrapper>(wrappers);
                eventAggregator.GetEvent<StatusBarEvent>()
                .Publish(new StatusBarInfo { ItemsCount = Files.Count() });
            }, ThreadOption.UIThread, true);

            eventAggregator.GetEvent<PlaylistEvent>().Subscribe(i =>
            {
                if (i.MessageType == MessageType.Remove)
                    Files = Files.Where(f => f.IsSelected).ToList();
            }, ThreadOption.UIThread, true);
        }

        private IEnumerable<FileAudioWrapper> _files = new List<FileAudioWrapper>();
        
        public IEnumerable<FileAudioWrapper> Files
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
