using PlaylistMaker.Commons;
using PlaylistMaker.Playlist.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PlaylistMaker.Playlist
{
    public class PlaylistModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Literals.playlist, typeof(ViewPlaylist));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}