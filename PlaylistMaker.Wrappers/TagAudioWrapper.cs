using PlaylistMaker.Models;
using System;

namespace PlaylistMaker.Wrappers
{
    public class TagAudioWrapper<TBase> : WrapperBase<TBase>
    {
        public Guid Id { get; }
        public bool HasTag
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Album
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Artyst
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string TrackNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string Genre
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Comment
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public TagType TagType
        {
            get => GetValue<TagType>();
            set => SetValue(value);
        }

        public TagVersion TagVersion
        {
            get => GetValue<TagVersion>();
            set => SetValue(value);
        }
    }
}
