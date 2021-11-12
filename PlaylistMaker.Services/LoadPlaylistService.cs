using PlaylistMaker.Commons;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistMaker.Services
{
    public class LoadPlaylistService : ILoadPlaylistService
    {
        public async Task<IResult<IEnumerable<string>>> LoadAsync(string path)
        {
            try
            {
                using var playlistFile = File.OpenText(path);
                string line = null;
                var playlist = new List<string>();
                while (!string.IsNullOrEmpty(line = await playlistFile.ReadLineAsync()))
                    playlist.Add(line);
                return Result.Ok(playlist.AsEnumerable());
            }
            catch
            {
                return Result.Error<IEnumerable<string>>("Loading playlist filed");
            }
        }

        public IResult<IEnumerable<string>> Load(string path)
            => TryCatchExtensions.TryCatch<IEnumerable<string>>(() =>
            {
                using var playlistFile = File.OpenText(path);
                string line = null;
                var playlist = new List<string>();
                while (!string.IsNullOrEmpty(line = playlistFile.ReadLine()))
                    playlist.Add(line);
                return playlist.AsEnumerable();
            });
    }
}
