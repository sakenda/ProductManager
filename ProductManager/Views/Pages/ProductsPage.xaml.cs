using ProductManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductManager
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            ListView_ProductList.Items.Clear();

            Database.Instance.GetFullDetailProducts();
            ListView_ProductList.ItemsSource = Database.Instance.CurrentProducts;

            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = false;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_ProductList.SelectedItem != null)
            {
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
        }

        //private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    var searchTextBox = sender as TextBox;
        //    if (searchTextBox != null && searchTextBox.Text == "Suchen...")
        //    {
        //        searchTextBox.Text = "";
        //        searchTextBox.Foreground = (Brush)TryFindResource("dSchriftv2");
        //    }
        //}

        //private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    var searchTextBox = sender as TextBox;
        //    if (searchTextBox != null && searchTextBox.Text == "")
        //    {
        //        searchTextBox.Text = "Suchen...";
        //        searchTextBox.Foreground = (Brush)TryFindResource("hSchriftv2");
        //    }
        //}
    }

    
}
