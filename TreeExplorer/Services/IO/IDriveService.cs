namespace ExplorerTreeView
{
    interface IDriveService
    {
        string GetLabel(string driveLetter);
        DriveType GetDriveType(string driveLetter);
        bool Exist(string driveLetter);
    }
}