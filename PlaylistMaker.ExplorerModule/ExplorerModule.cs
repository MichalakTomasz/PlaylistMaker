using PlaylistMaker.Commons;
using PlaylistMaker.Explorer.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PlaylistMaker.Explorer
{
    public class ExplorerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Literals.explorer, typeof(ViewA));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}