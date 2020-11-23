using ProductManager.ViewModels;
using ProductManager.Models;
using ProductManager.Views.Validation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProductManager.Models.Database;

namespace ProductManager
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            ListView_ProductList.Items.Clear();
            InitializeComboBoxMetaData();

            Database.Instance.GetFullDetailProducts();
            ListView_ProductList.ItemsSource = Database.Instance.CurrentProducts;

            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = false;
        }

        #region Events
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_ProductList.SelectedItem is ProductFullDetail selectedProduct)
            {
                cb_Supplier.SelectedValue = selectedProduct.SupplierData.DataID;
                cb_Category.SelectedValue = selectedProduct.CategoryData.DataID;

                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                cb_Supplier.SelectedValue = -1;
                cb_Category.SelectedValue = -1;

                btnDelete.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
        }

        private void cb_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ListView_ProductList.SelectedItem as ProductFullDetail;
            var selectedCategory = cb_Category.SelectedItem as CategoryData;

            if (selectedProduct != null && selectedCategory != null && selectedProduct.CategoryData.DataID != selectedCategory.DataID)
            {
                selectedProduct.CategoryData.DataID = selectedCategory.DataID;
                selectedProduct.CategoryData.CategoryName = selectedCategory.CategoryName;
            }
        }

        private void cb_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ListView_ProductList.SelectedItem as ProductFullDetail;
            var selectedSupplier = cb_Supplier.SelectedItem as SupplierData;

            if (selectedProduct != null && selectedSupplier != null && selectedProduct.SupplierData.DataID != selectedSupplier.DataID)
            {
                selectedProduct.SupplierData.DataID = selectedSupplier.DataID;
                selectedProduct.SupplierData.SupplierName = selectedSupplier.SupplierName;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var searchTextBox = sender as TextBox;
            if (searchTextBox != null && searchTextBox.Text == "Suchen...")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = (Brush)TryFindResource("dSchriftv2");
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var searchTextBox = sender as TextBox;
            if (searchTextBox != null && searchTextBox.Text == "")
            {
                searchTextBox.Text = "Suchen...";
                searchTextBox.Foreground = (Brush)TryFindResource("hSchriftv2");
            }
        }
        #endregion

        #region Clicks
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            BindingGroup.CommitEdit();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.BindingGroup.CancelEdit();
        }
        #endregion

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
        #endregion

    }


}
