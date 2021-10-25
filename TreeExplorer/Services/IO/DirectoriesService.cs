using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace ExplorerTreeView
{
    static class DirectoriesService
    {
        public static IEnumerable<string> GetFolders(string path, bool fullPath = false)
        {
            try
            {
                if (new DriveInfo(path).IsReady)
                {
                    if (fullPath)
                    {
                        return new DirectoryInfo(path)
                        .EnumerateDirectories()
                        .Where(d => (d.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)
                        .Select(f => f.FullName)
                        .ToList();
                    }
                    else
                    {
                        return new DirectoryInfo(path)
                        .EnumerateDirectories()
                        .Where(d => (d.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)
                        .Select(f => f.Name)
                        .ToList();
                    }
                }
                else return Enumerable.Empty<string>();
            }
            catch (DirectoryNotFoundException)
            {
                return Enumerable.Empty<string>();
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<string>();
            }
            catch (SecurityException)
            {
                return Enumerable.Empty<string>();
            }
        }

        public static int GetFoldersCount(string path)
        {
            try
            {
                if (new DriveInfo(path).IsReady)
                {
                    return new DirectoryInfo(path)
                        .GetDirectories()
                        .Where(d => (d.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)
                        .Count();
                }
                else return 0;
            }
            catch (DirectoryNotFoundException)
            {
                return 0;
            }
            catch (UnauthorizedAccessException)
            {
                return 0;
            }
            catch (SecurityException)
            {
                return 0;
            }
        }

        public static bool Exist(string path)
        {
            return Directory.Exists(path);
        }

        public static string GetFolderName(string path)
        {
            return Path.GetDirectoryName(path);
        }
    }
}
