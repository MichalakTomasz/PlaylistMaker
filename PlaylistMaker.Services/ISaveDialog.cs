namespace PlaylistMaker.Services
{
    public interface ISaveDialog
    {
        string SelectPath();
        public string Filter { get; set; }
    }
}