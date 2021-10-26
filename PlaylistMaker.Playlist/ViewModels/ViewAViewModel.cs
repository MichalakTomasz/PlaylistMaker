using PlaylistMaker.Events;
using PlaylistMaker.Wrappers;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlaylistMaker.Playlist.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        public ViewAViewModel(IEventAggregator eventAggregator)
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
    }
}
