using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExplorerTreeView
{
    public class Node : DependencyObject
    {
        public Node(string filesFilter = null)
            => FilesFilter = filesFilter;

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register(
                "Path", 
                typeof(string), 
                typeof(Node), 
                new PropertyMetadata(null));

        public IEnumerable<string> FoldersNames
        {
            get
            {
                if (!FileService.IsFile(Path) && DirectoriesService.Exist(Path))
                    FoldersNames = DirectoriesService.GetFolders(Path);
                else FoldersNames = Enumerable.Empty<string>();

                return (IEnumerable<string>)GetValue(FoldersNamesProperty);
            }
            set { SetValue(FoldersNamesProperty, value); }
        }

        public static readonly DependencyProperty FoldersNamesProperty =
            DependencyProperty.Register(
                "FoldersNames", 
                typeof(IEnumerable<string>), 
                typeof(Node), 
                new PropertyMetadata(null));
        
        public IEnumerable<string> FoldersFullPath
        {
            get
            {
                if (!FileService.IsFile(Path) && DirectoriesService.Exist(Path))
                    FoldersFullPath = DirectoriesService.GetFolders(Path, fullPath: true);
                else FoldersFullPath = Enumerable.Empty<string>();

                return (IEnumerable<string>)GetValue(FoldersFullPathProperty);
            }
            set { SetValue(FoldersFullPathProperty, value); }
        }

        public static readonly DependencyProperty FoldersFullPathProperty =
            DependencyProperty.Register(
                "FoldersFullPath", 
                typeof(IEnumerable<string>), 
                typeof(Node), 
                new PropertyMetadata(null));
                
        public IEnumerable<string> FilesNames
        {
            get
            {
                if (!FileService.IsFile(Path) && DirectoriesService.Exist(Path))
                {
                    if (!string.IsNullOrWhiteSpace(FilesFilter))
                        FilesNames = FileService.GetFiles(Path, FilesFilter);
                    else
                        FilesNames = FileService.GetFiles(Path);
                }
                else FilesNames = Enumerable.Empty<string>();

                return (IEnumerable<string>)GetValue(FilesNamesProperty);
            }
            set { SetValue(FilesNamesProperty, value); }
        }

        public static readonly DependencyProperty FilesNamesProperty =
            DependencyProperty.Register(
                "FilesNames", 
                typeof(IEnumerable<string>), 
                typeof(Node), 
                new PropertyMetadata(null));

        public IEnumerable<string> FilesFullPath
        {
            get
            {
                if (!FileService.IsFile(Path) && DirectoriesService.Exist(Path))
                {
                    if (!string.IsNullOrWhiteSpace(FilesFilter))
                        FilesFullPath = FileService.GetFiles(Path, FilesFilter, fullPath: true);
                    else
                        FilesFullPath = FileService.GetFiles(Path, fullPath: true);
                }
                else FilesFullPath = Enumerable.Empty<string>();

                return (IEnumerable<string>)GetValue(FilesFullPathProperty);
            }
            set { SetValue(FilesFullPathProperty, value); }
        }

        public static readonly DependencyProperty FilesFullPathProperty =
            DependencyProperty.Register(
                "FilesFullPath", 
                typeof(IEnumerable<string>), 
                typeof(Node), 
                new PropertyMetadata(null));

        private string FilesFilter { get; }
    }
}
