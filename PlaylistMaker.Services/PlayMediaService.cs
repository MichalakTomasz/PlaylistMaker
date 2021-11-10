using PlaylistMaker.Commons;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PlaylistMaker.Services
{
    public class PlayMediaService : IPlayMediaService
    {
        private MediaPlayer _mediaPlayer;

        public IResult Open(string fullPath)
            => TryCatchExtensions.TryCatch(() =>
            {
                if (IsOpened) _mediaPlayer.Close();
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Open(new Uri(fullPath));
            });

        public bool IsOpened => _mediaPlayer != default;

        public IResult Play() => TryCatchExtensions.TryCatch(() => _mediaPlayer?.Play());
        public IResult Pause() => TryCatchExtensions.TryCatch(() => _mediaPlayer?.Pause());
        public IResult Stop() => TryCatchExtensions.TryCatch(() => _mediaPlayer?.Stop());
        public double Volume {
            get => GetPlayerValue<double>(nameof(MediaPlayer.Volume));
            set => SetPlayerValue(value, nameof(MediaPlayer.Volume));
        }

        private TResult GetPlayerValue<TResult>(string propertyName)
        {
            MediaPlayer mediaPlayer = null;
            if (IsOpened)
                mediaPlayer = _mediaPlayer;
            else mediaPlayer = new MediaPlayer();

            return (TResult)typeof(MediaPlayer).GetProperty(propertyName).GetValue(mediaPlayer);
        }

        private void SetPlayerValue<T>(T value, string propertyName)
        {
            MediaPlayer mediaPlayer = null;
            if (IsOpened)
                mediaPlayer = _mediaPlayer;
            else
                mediaPlayer = new MediaPlayer();
                
            typeof(MediaPlayer).GetProperty(propertyName).SetValue(mediaPlayer, value);
        }

        public double Balance 
        { 
            get => GetPlayerValue<double>(nameof(MediaPlayer.Balance)); 
            set => SetPlayerValue(value, nameof(MediaPlayer.Balance)); 
        }
        public bool IsMuted 
        
        { 
            get => GetPlayerValue<bool>(nameof(MediaPlayer.IsMuted)); 
            set => SetPlayerValue(value, nameof(MediaPlayer.IsMuted)); 
        }
        public TimeSpan Position 
        { 
            get => GetPlayerValue<TimeSpan>(nameof(MediaPlayer.Position)); 
            set => SetPlayerValue(value, nameof(MediaPlayer.Position)); 
        }
    }
}
