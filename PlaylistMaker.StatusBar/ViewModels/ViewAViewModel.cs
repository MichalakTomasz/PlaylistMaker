using PlaylistMaker.Events;
using Prism.Events;
using Prism.Mvvm;

namespace PlaylistMaker.StatusBar.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ViewAViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<StatusBarEvent>()
                .Subscribe(e => ItemsCount = e.ItemsCount);
        }

        private int _itemsCount;
        public int ItemsCount
        {
            get { return _itemsCount; }
            set { SetProperty(ref _itemsCount, value); }
        }
    }
}
