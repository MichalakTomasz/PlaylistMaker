using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ExplorerTreeView
{
    interface IFileService
    {
        IEnumerable<string> GetFiles(string path, string filesFilter = null);
        string GetExtension(string path);
        bool Exist(string path);
        string GetFileName(string path);
        int GetFilesCount(string path);
        bool IsFile(string path);
        BitmapSource GetIconFromFilePath(string path);
    }
}