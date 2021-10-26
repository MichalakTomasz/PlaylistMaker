using PlaylistMaker.Commons;
using PlaylistMaker.Files.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PlaylistMaker.Files
{
    public class FilesModule : IModule
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