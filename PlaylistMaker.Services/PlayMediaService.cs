using PlaylistMaker.Commons;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlaylistMaker.Services
{
    public class PlayMediaService : IPlayMediaService
    {
        private MediaPlayer _mediaPlayer;
        private Task _progress;

        public IResult Open(string fullPath)
            => TryCatchExtensions.TryCatch(() =>
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Open(new Uri(fullPath)); 
            });

        public IResult Play() => TryCatchExtensions.TryCatch(() =>
        {
            _mediaPlayer.Play();
            _progress = Task.Run(() =>
            {

            });
        });
        public IResult Pause() => TryCatchExtensions.TryCatch(() => _mediaPlayer.Pause());
        public IResult Stop() => TryCatchExtensions.TryCatch(() => _mediaPlayer.Stop());
        public IResult Volume(int value) => TryCatchExtensions.TryCatch(() => _mediaPlayer.Volume = value);
        public IResult Balance(double value) => TryCatchExtensions.TryCatch(() => _mediaPlayer.Balance = value);
        public bool IsMuted { get => _mediaPlayer.IsMuted; set => _mediaPlayer.IsMuted = value; }
        public TimeSpan Position { get => _mediaPlayer.Position; set => _mediaPlayer.Position = value; }            
    }
}
