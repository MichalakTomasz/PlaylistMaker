using Microsoft.Win32;

namespace PlaylistMaker.Services
{
    public class SaveDialog : ISaveDialog
    {
        public string SelectPath()
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "Playlist (*.m3u)| *.m3u"
            };

            saveDialog.ShowDialog();
            return saveDialog.FileName;
        }
    }
}
