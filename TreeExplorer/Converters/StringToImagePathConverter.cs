using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ExplorerTreeView
{
    class StringToImagePathConverter :
        IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            switch (value.ToString())
            {
                case "Root":
                    return GetImageFromName(value.ToString());
                case "Hdd":
                    return GetImageFromName(value.ToString());
                case "CdRom":
                    return GetImageFromName(value.ToString());
                case "Folder":
                    return GetImageFromName(value.ToString());
                case "Network":
                    return GetImageFromName(value.ToString());
                case "Unknown":
                    return GetImageFromName("Hdd");
                default:
                    return GetImageFromPath(value.ToString());
            }
        }

        private BitmapSource GetImageFromName(string name)
        {
            //var resource = Application.Current.TryFindResource(name);
            //if (resource != default)
            //{
            //    var bit = (BitmapSource)resource;
            //    return bit;
            //}
            //return default;
            var img = name.Substring(0, 1) != "." ? new BitmapImage(new Uri($"/PlaylistMaker/TreeExplorer/Images;ContentFile/Images/{name}.ico", UriKind.Relative))
                : new BitmapImage(new Uri($"/PlaylistMaker/TreeExplorer/Images;ContentFile/File.ico", UriKind.Relative));
            return img;
        }

        private BitmapSource GetImageFromPath(string path)
        {
            var extension = FileService.GetExtension(path);
            BitmapSource resourceImage = (BitmapSource)Application.Current.TryFindResource(extension);
            if (resourceImage == null)
            {
                var image = FileService.GetIconFromFilePath(path);
                Application.Current.Resources.Add(extension, image);
                return image;
            }
            else return resourceImage;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}