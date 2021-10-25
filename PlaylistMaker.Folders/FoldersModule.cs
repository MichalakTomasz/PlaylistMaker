using PlaylistMaker.Commons;
using PlaylistMaker.Folders.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PlaylistMaker.Folders
{
    public class FoldersModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Literals.folders, typeof(ViewA));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}