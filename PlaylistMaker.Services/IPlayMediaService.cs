using PlaylistMaker.Commons;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PlaylistMaker.Services
{
    public interface IPlayMediaService
    {
        IResult Open(string fullPath);
        IResult Pause();
        IResult Play();
        IResult Stop();
        double Balance { get; set; }
        double Volume { get; set; }
        bool IsMuted { get; }
        TimeSpan Position { get; set; }
        bool IsOpened { get; }
    }
}