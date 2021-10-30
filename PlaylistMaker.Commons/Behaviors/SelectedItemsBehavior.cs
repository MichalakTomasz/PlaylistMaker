using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PlaylistMaker.Commons.Behaviors
{
    public class SelectedItemsBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
            => AssociatedObject.SelectionChanged += DataGrid_SelectionChanged;

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => SelectedItems = AssociatedObject.SelectedItems;

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(SelectedItemsBehavior), new PropertyMetadata(null));
    }
}
