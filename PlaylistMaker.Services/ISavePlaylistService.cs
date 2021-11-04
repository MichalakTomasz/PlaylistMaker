using PlaylistMaker.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaylistMaker.Services
{
    public interface ISavePlaylistService
    {
        Task<IResult> SaveAsync(IEnumerable<string> pathList, string destinationPath);
    }
}