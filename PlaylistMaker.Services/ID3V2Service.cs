using AutoMapper;
using IdSharp.Tagging.ID3v2;
using PlaylistMaker.Commons;
using PlaylistMaker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PlaylistMaker.Services
{
    public class ID3V2Service : IID3Service, IID3V2ImageService
    {
        private readonly IID3Genres _iD3Genres;
        private readonly IMapper _mapper;

        public ID3V2Service(IID3Genres iD3Genres, IMapper mapper)
        {
            _iD3Genres = iD3Genres;
            _mapper = mapper;
        }
        public bool HasTag(string filePath)
            => ID3v2Tag.DoesTagExist(filePath);

        public TagAudio GetTag(string filePath)
        {
            var hasTag = ID3v2Tag.DoesTagExist(filePath);

            if (hasTag)
            {
                var tag = new ID3v2Tag(filePath);
                var audioFile = _mapper.Map(tag, new TagAudio());
                audioFile.HasTag = true;
                audioFile.TagType = TagType.ID3V2;

                return audioFile;
            }
            else
            {
                return new TagAudio
                {
                    HasTag = false
                };
            }
        }

        public TagVersion GetTagVersion(string filePath)
            => TagVersion.unknown;

        public IReadOnlyList<string> GetGenres()
            => _iD3Genres.Genres;

        public void UpdateTag(TagAudio audioFile, string filePath)
            => TryCatchExtensions.TryCatch(() =>
            {
                var id3v2Tag = _mapper.Map(audioFile, new ID3v2Tag(filePath));
                id3v2Tag.Save(filePath);
            }, "ID3v2 saving error");

        public void RemoveTag(string filePath)
            => TryCatchExtensions.TryCatch(() => ID3v2Tag.RemoveTag(filePath), "ID3v2 removing error");

        #region IID3V2ImageService

        public IResult<bool> HasImages(string filepath)
            => TryCatchExtensions.TryCatch(() =>
            {
                if (!ID3v2Tag.DoesTagExist(filepath))
                    return false;

                var tag = new ID3v2Tag(filepath);
                return tag.PictureList?.Count > 0;
            });

        public IResult<IEnumerable<BitmapImage>> GetImages(string filePath)
            => TryCatchExtensions.TryCatch(() =>
            {
                if (!ID3v2Tag.DoesTagExist(filePath))
                    return default;

                var tag = new ID3v2Tag(filePath);
                if (tag.PictureList?.Count == 0)
                    return default;

                return tag.PictureList.Select(p => ConvertToBitmapImage(p.PictureData))
                .ToList().AsEnumerable();
            }, "Loading images filed");

        private BitmapImage ConvertToBitmapImage(byte[] buffer)
        {
            using (var memoryStream = new MemoryStream(buffer))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
                return image;
            }
        }

        #endregion IID3V2ImageService

    }
}
