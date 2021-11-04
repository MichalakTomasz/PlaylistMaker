using PlaylistMaker.Commons;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PlaylistMaker.Services
{
    public class SavePlaylistService : ISavePlaylistService
    {
        public async Task<IResult> SaveAsync(IEnumerable<string> pathList, string destinationPath)
        {
            using var writer = File.CreateText(destinationPath);
            foreach (var path in pathList)
                await writer.WriteLineAsync(path);

            return Result.Ok();
        }
    }
}
