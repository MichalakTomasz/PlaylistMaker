using PlaylistMaker.Commons;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace PlaylistMaker.Services
{
    public interface IID3V2ImageService
    {
        IResult<bool> HasImages(string filepath);
        IResult<IEnumerable<BitmapImage>> GetImages(string filepath);
    }
}
