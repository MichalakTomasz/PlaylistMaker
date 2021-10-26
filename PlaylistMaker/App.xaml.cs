using AutoMapper;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using PlaylistMaker.Commons;
using PlaylistMaker.Explorer;
using PlaylistMaker.Files;
using PlaylistMaker.Models;
using PlaylistMaker.Playlist;
using PlaylistMaker.Services;
using PlaylistMaker.Views;
using PlaylistMaker.Wrappers;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace PlaylistMaker
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IID3Genres, ID3V1Genres>();
            containerRegistry.Register<IID3Service, ID3V1Service>(Literals.id3v1);
            containerRegistry.Register<IID3Service, ID3V2Service>(Literals.id3v2);
            var imapper = CreateMapper();
            containerRegistry.RegisterInstance(imapper);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ExplorerModule>();
            moduleCatalog.AddModule<FilesModule>();
            moduleCatalog.AddModule<PlaylistModule>();
        }

        IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.CreateMap<TagAudio, ID3v1Tag>();
                c.CreateMap<TagAudio, ID3v2Tag>();
                c.CreateMap<ID3v1Tag, TagAudio>();
                c.CreateMap<ID3v2Tag, TagAudio>();
            });
            return mapperConfig.CreateMapper();
        }
    }
}
