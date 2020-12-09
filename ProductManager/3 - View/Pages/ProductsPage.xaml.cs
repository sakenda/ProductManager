using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProductManager.ViewModel;

namespace ProductManager
{
    public partial class ProductsPage : Page
    {
        private MainProductsViewModel vm;

        public ProductsPage()
        {
            vm = (MainProductsViewModel)this.TryFindResource("vmProducts");
            if (vm != null)
            {
                this.CommandBindings.Add(vm.NewCommandBinding);
                this.CommandBindings.Add(vm.SaveCommandBinding);
                this.CommandBindings.Add(vm.DeleteCommandBinding);
            }
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            var searchTextBox = sender as TextBox;
            if (searchTextBox != null && searchTextBox.Text == "Suchen...")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = (Brush)TryFindResource("dSchriftv2");
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            var searchTextBox = sender as TextBox;
            if (searchTextBox != null && searchTextBox.Text == "")
            {
                searchTextBox.Text = "Suchen...";
                searchTextBox.Foreground = (Brush)TryFindResource("hSchriftv2");
            }
        }
    }
}