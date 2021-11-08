namespace PlaylistMaker.Models
{
    public class MainWindowInfo
    {
        public bool CanPlay { get; set; }
        public bool CanSavePlaylist { get; set; }
        public MessageType MessageType { get; set; }
        public object MessageValue { get; set; }
    }
}
