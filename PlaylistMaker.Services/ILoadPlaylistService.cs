using PlaylistMaker.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaylistMaker.Services
{
    public interface ILoadPlaylistService
    {
        Task<IResult<IEnumerable<string>>> LoadAsync(string path);
        IResult<IEnumerable<string>> Load(string path);
    }
}