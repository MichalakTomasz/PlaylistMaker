using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Unity;

namespace PlaylistMaker.Wrappers
{
    public class FileAudioWrapper : WrapperBase<FileAudio> 
    {
        private readonly IPlayMediaService _playMediaService;

        public FileAudioWrapper(string fullPath, 
            [Dependency(Literals.id3v1)] IID3Service id3V1Service, 
            [Dependency(Literals.id3v2)] IID3Service Id3V2Service,
            IPlayMediaService playMediaService)
        {
            _playMediaService = playMediaService;
            Model = new FileAudio
            {
                ID3v1Tag = id3V1Service.GetTag(fullPath),
                ID3v2Tag = Id3V2Service.GetTag(fullPath),
                Filename = Path.GetFileName(fullPath),
                FullPath = fullPath,
            };

            ID3v1Tag = new TagId3v1Wrapper(Model.ID3v1Tag);
            ID3v2Tag = new TagId3v2Wrapper(Model.ID3v2Tag);
        }

        public FileAudioWrapper(FileAudio fileAudio)
            => Model = fileAudio;

        public string FullPath
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Filename 
        { 
            get => GetValue<string>(); 
            set => SetValue(value); 
        }
        public TagId3v1Wrapper ID3v1Tag { get; set; }
        public TagId3v2Wrapper ID3v2Tag { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotyfyPropertyChanged();
            }
        }
    }
}
