namespace PlaylistMaker.Services
{
    public interface ILoadDialog
    {
        string Load();
        public string Filter { get; set; }
    }
}