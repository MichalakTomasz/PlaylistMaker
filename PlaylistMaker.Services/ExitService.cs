using System.Windows;

namespace PlaylistMaker.Services
{
    public class ExitService : IExitService
    {
        public void Exit() => Application.Current.Shutdown();
    }
}
