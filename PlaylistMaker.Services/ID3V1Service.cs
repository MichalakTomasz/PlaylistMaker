using PlaylistMaker.Models;
using AutoMapper;
using IdSharp.Tagging.ID3v1;
using PlaylistMaker.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlaylistMaker.Services
{
    public class ID3V1Service : IID3Service
    {
        private readonly IID3Genres _id3Genres;
        private readonly IMapper _mapper;

        public ID3V1Service(IID3Genres id3Genres, IMapper mapper) 
        {
            _id3Genres = id3Genres;
            _mapper = mapper;
        }

        public IReadOnlyList<string> GetGenres()
            => _id3Genres.Genres;

        public TagAudio GetTag(string filePath)
        {
            var hasTag = ID3v1Tag.DoesTagExist(filePath);
            var genres = _id3Genres.Genres;

            if (hasTag)
            {
                var tag = new ID3v1Tag(filePath);
                var audioFile = _mapper.Map(tag, new TagAudio());
                audioFile.HasTag = true;
                audioFile.TagType = TagType.ID3V1;
                audioFile.TagVersion = GetTagVersion(filePath);
                audioFile.Genre = genres[tag.GenreIndex];

                return audioFile;
            }
            else
            {
                return new TagAudio
                {
                    HasTag = false,
                };
            }
        }

        public TagVersion GetTagVersion(string filePath)
        {
            if (HasTag(filePath))
            {
                var tag = new ID3v1Tag(filePath);

                switch (tag.TagVersion)
                {
                    case ID3v1TagVersion.ID3v10:
                        return TagVersion.ID3V10;
                    case ID3v1TagVersion.ID3v11:
                        return TagVersion.ID3V11;
                }
            }
            return TagVersion.none;
        }

        public bool HasTag(string filePath) => ID3v1Tag.DoesTagExist(filePath);

        public void RemoveTag(string filePath)
            => TryCatchExtensions.TryCatch(() => ID3v1Tag.RemoveTag(filePath), "ID3v1 removing error");

        public void UpdateTag(TagAudio audioFile, string filePath)
        {
            var genres = _id3Genres.Genres;
            var genreIndex = genres.ToList().FindIndex(g => g == audioFile.Genre);

            var id3v1Tag = _mapper.Map(audioFile, new ID3v1Tag(filePath));
            id3v1Tag.GenreIndex = genreIndex;
            TryCatchExtensions.TryCatch(() => id3v1Tag.Save(filePath), "IDv1 updating error");
        }
    }
}
