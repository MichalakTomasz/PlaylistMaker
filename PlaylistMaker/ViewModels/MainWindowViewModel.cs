using PlaylistMaker.Commons;
using PlaylistMaker.Services;
using Prism.Mvvm;

namespace PlaylistMaker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IID3Service iD3Service;

        public MainWindowViewModel()
        {
        }
    }
}
