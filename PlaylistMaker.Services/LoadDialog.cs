using Microsoft.Win32;
namespace PlaylistMaker.Services
{
    public class LoadDialog : ILoadDialog
    {
        public string Load()
        {
            var dialog = new OpenFileDialog
            {
                Filter = Filter
            };

            dialog.ShowDialog();
            return dialog.FileName;
        }

        public string Filter { get; set; }
    }
}
