using PlaylistMaker.Events;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using PlaylistMaker.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistMaker.Playlist.ViewModels
{
    public class ViewPlaylistViewModel : BindableBase
    {
        public ViewPlaylistViewModel(
            IEventAggregator eventAggregator,
            ISavePlaylistService savePlaylistService,
            ISaveDialog saveDialog)
        {
            _eventAggregator = eventAggregator;
            _savePlaylistService = savePlaylistService;
            _saveDialog = saveDialog;
            DoEvents();
        }

        private void DoEvents()
        {
            _eventAggregator.GetEvent<SelectionEvent>().Subscribe(files =>
            {
                var wrappers = files.Select(f => new FileAudioWrapper(f)).ToList();
                Files = Files.Concat(wrappers).ToList();
                SendItemsCount();
            }, ThreadOption.UIThread, true);

            _eventAggregator.GetEvent<PlaylistEvent>().Subscribe(i =>
            {
                if (i.MessageType == MessageType.Remove)
                {
                    RemoveCheckedItemsCommand.Execute();
                    SendItemsCount();
                }
            }, ThreadOption.UIThread, true);

            _eventAggregator.GetEvent<MainWindowEvent>().Subscribe(i =>
            {
                if (i.MessageType == MessageType.SavePlaylist)
                    SavePlaylistCommand.Execute();
            });
        }

        private void SendItemsCount()
            => _eventAggregator.GetEvent<StatusBarEvent>()
            .Publish(new StatusBarInfo { ItemsCount = Files.Count() });

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

        private IList _selectedItems;
        public IList SelectedItems
        {
            get { return _selectedItems; }
            set { SetProperty(ref _selectedItems, value); }
        }

        private bool _isEnabledRemoveItem;
        public bool IsEnabledRemoveItem
        {
            get { return _isEnabledRemoveItem; }
            set { SetProperty(ref _isEnabledRemoveItem, value); }
        }

        private bool _isEnabledCheckItem;
        public bool IsEnabledCheckItem
        {
            get { return _isEnabledCheckItem; }
            set { SetProperty(ref _isEnabledCheckItem, value); }
        }

        private bool _isEnabledUncheckItem;
        public bool IsEnabledUncheckItem
        {
            get { return _isEnabledUncheckItem; }
            set { SetProperty(ref _isEnabledUncheckItem, value); }
        }

        private bool _isEnabledSavePlaylist;
        public bool IsEnabledSavePlaylist
        {
            get { return _isEnabledSavePlaylist; }
            set { SetProperty(ref _isEnabledSavePlaylist, value); }
        }

        private bool _isEnabledRemoveCheckedItems;
        public bool IsEnabledRemoveCheckedItems
        {
            get { return _isEnabledRemoveCheckedItems; }
            set { SetProperty(ref _isEnabledRemoveCheckedItems, value); }
        }

        private bool _isEnabledCheckAll;
        public bool IsEnabledCheckAll
        {
            get { return _isEnabledCheckAll; }
            set { SetProperty(ref _isEnabledCheckAll, value); }
        }

        private bool _isEnabledUncheckAll;
        public bool IsEnabledUncheckAll
        {
            get { return _isEnabledUncheckAll; }
            set { SetProperty(ref _isEnabledUncheckAll, value); }
        }

        private DelegateCommand _selectAllCommand;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISavePlaylistService _savePlaylistService;
        private readonly ISaveDialog _saveDialog;

        public DelegateCommand SelectAllCommand =>
            _selectAllCommand ?? (_selectAllCommand = 
            new DelegateCommand(ExecuteSelectAllCommand, () => Files.Any()))
            .ObservesProperty(() => Files);

        void ExecuteSelectAllCommand()
        {
            if (SelectAll) Files.ToList().ForEach(f => f.IsSelected = true);
            else Files.ToList().ForEach(f => f.IsSelected = false);
        }

        private DelegateCommand _removeItemCommand;
        public DelegateCommand RemoveItemCommand =>
            _removeItemCommand ?? (_removeItemCommand = 
            new DelegateCommand(ExecuteRemoveItemCommand));

        void ExecuteRemoveItemCommand()
        {
            var selected = SelectedItems.Cast<FileAudioWrapper>().FirstOrDefault();
            if (selected != default)
                Files = Files.Where(f => f != selected).ToList();
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

        private DelegateCommand _removeCheckedItemsCommmand;
        public DelegateCommand RemoveCheckedItemsCommand =>
            _removeCheckedItemsCommmand ?? (_removeCheckedItemsCommmand = 
            new DelegateCommand(ExecuteRemoveCheckedItemsCommand));

        void ExecuteRemoveCheckedItemsCommand()
        {
            Files = Files.Where(f => !f.IsSelected).ToList();
            SelectAll = false;
        }

        private DelegateCommand _checkAllCommand;
        public DelegateCommand CheckAllCommand =>
            _checkAllCommand ?? (_checkAllCommand = new DelegateCommand(() =>
            {
                SelectAll = true;
                SelectAllCommand.Execute();
            }));

        private DelegateCommand _unchekcAllCommand;
        public DelegateCommand UncheckAllCommand =>
            _unchekcAllCommand ?? (_unchekcAllCommand = new DelegateCommand(() =>
            {
                SelectAll = false;
                SelectAllCommand.Execute();
            }));

        private DelegateCommand _contextMenuOpenedCommand;
        public DelegateCommand ContextMenuOpenedCommand =>
            _contextMenuOpenedCommand ?? (_contextMenuOpenedCommand = 
            new DelegateCommand(() => IsEnabledRemoveItem = IsEnabledCheckItem = 
            IsEnabledUncheckItem = IsEnabledSavePlaylist = IsEnabledRemoveCheckedItems = 
            IsEnabledCheckAll = IsEnabledUncheckAll = Files.Any()));

        private DelegateCommand _savePlaylistCommand;
        public DelegateCommand SavePlaylistCommand =>
            _savePlaylistCommand ?? (_savePlaylistCommand = 
            new DelegateCommand(() =>
            {
                var path = _saveDialog.SelectPath();
                var pathList = Files.Select(f => f.FullPath);
                _savePlaylistService.SaveAsync(pathList, path);
            }));
    }
}
