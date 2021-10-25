using AutoMapper;
using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using PlaylistMaker.Services;
using System;
using Unity;

namespace Playlist.Wrappers
{
    public class FileAudioWrapper : WrapperBase<FileAudioWrapper>
    {

        public FileAudioWrapper(string fullPath, [Dependency(Literals.id3v1)] IID3Service id3V1Service, [Dependency(Literals.id3v2)]IID3Service Id3V2Service) : base()
        {
            Id3v1 = new TagId3v1Wrapper(fullPath, Id3V2Service);
            Id3v2 = new TagId3v2Wrapper(fullPath, Id3V2Service);
        }

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

        public TagId3v1Wrapper Id3v1
        {
            get => GetValue<TagId3v1Wrapper>();
            set => SetValue(value);
        } 

        public TagId3v2Wrapper Id3v2
        {
            get => GetValue<TagId3v2Wrapper>();
            set => SetValue(value);
        }
    }
}
