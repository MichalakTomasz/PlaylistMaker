using PlaylistMaker.Commons;
using System;
using System.Threading.Tasks;

namespace PlaylistMaker.Services
{
    public interface IPlayMediaService
    {
        IResult Balance(double value);
        IResult Open(string fullPath);
        IResult Pause();
        IResult Play();
        IResult Stop();
        IResult Volume(int value);
        bool IsMuted { get; }
        TimeSpan Position { get; set; }
    }
}