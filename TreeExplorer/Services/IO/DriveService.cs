using System.IO;
namespace ExplorerTreeView
{
    static class DriveService
    {
        public static string GetLabel(string driveLetter)
        {
            var drive = new DriveInfo(driveLetter);
            return drive.IsReady ? drive.VolumeLabel : "";
        }
        
        public static DriveType GetDriveType(string driveLetter)
        {
            var drives = new DriveInfo(driveLetter);
            switch (drives.DriveType)
            {
                case System.IO.DriveType.Unknown:
                    return DriveType.Unknown;
                case System.IO.DriveType.NoRootDirectory:
                    return DriveType.Unknown;
                case System.IO.DriveType.Removable:
                    return DriveType.Hdd;
                case System.IO.DriveType.Fixed:
                    return DriveType.Hdd;
                case System.IO.DriveType.Network:
                    return DriveType.Network;
                case System.IO.DriveType.CDRom:
                    return DriveType.CdRom;
                case System.IO.DriveType.Ram:
                    return DriveType.Unknown;
                default:
                    return DriveType.Unknown;
            }
        }

        public static bool Exist(string path)
        {
            return Directory.Exists(path);
        }
    }
}
