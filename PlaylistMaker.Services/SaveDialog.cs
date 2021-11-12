using Microsoft.Win32;

namespace PlaylistMaker.Services
{
    public class SaveDialog : ISaveDialog
    {
        public string SelectPath()
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = Filter
            };

            saveDialog.ShowDialog();
            return saveDialog.FileName;
        }

        public string Filter { get; set; }
    }
}
