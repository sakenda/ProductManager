using ProductManager.Models;
using ProductManager.Models.Database;
using ProductManager.ViewModels;
using ProductManager.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductManager
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();

            InitializeComboBoxMetaData();
            Database.Instance.GetFullDetailProducts();
            ListView_ProductList.ItemsSource = Database.Instance.CurrentProducts;
        }

        #region Events

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

        #endregion Events

        #region Clicks

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ProductDetailsRoot.BindingGroup.CommitEdit();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ProductDetailsRoot.BindingGroup.CancelEdit();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewProductWindow newProductWindow = new NewProductWindow();
            newProductWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListView_ProductList.SelectedItem is ProductFullDetail selectedProduct)
            {
                Database.Instance.DeletedProducts.Add(selectedProduct);
                Database.Instance.CurrentProducts.Remove(selectedProduct);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Sollen die änderungen zum Server gesendet werden?",
                                                      "Speichern...",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Database.Instance.SaveProductList();
                    Database.Instance.GetFullDetailProducts();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        #endregion Clicks

        #region Methoden

        private void InitializeComboBoxMetaData()
        {
            cb_Category.ItemsSource = DatabaseMetaData.Instance.CategoryList;
            cb_Category.DisplayMemberPath = nameof(CategoryData.CategoryName);
            cb_Category.SelectedValuePath = nameof(CategoryData.DataID);

            cb_Supplier.ItemsSource = DatabaseMetaData.Instance.SupplierList;
            cb_Supplier.DisplayMemberPath = nameof(SupplierData.SupplierName);
            cb_Supplier.SelectedValuePath = nameof(SupplierData.DataID);

            if (cb_Category.Items.Count > 0)
            {
                cb_Category.SelectedIndex = -1;
            }

            if (cb_Supplier.Items.Count > 0)
            {
                cb_Supplier.SelectedIndex = -1;
            }
        }

        #endregion Methoden
    }
}