using ProductManager.Models.Product;
using ProductManager.Models.Product.Metadata;
using ProductManager.ViewModels.DatabaseData;
using ProductManager.Views;
using System;
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

        private void btnNew_Click(object sender, RoutedEventArgs e) => new NewProductWindow().ShowDialog();

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbProducts.SelectedItem is ProductFullDetail selectedProduct)
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

        private void FilterCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbProducts != null)
            {
                var selectedSupplierID = ((SupplierData)filter_cbSupplier.SelectedItem).ID_Supplier;
                var selectedCategoryID = ((CategoryData)filter_cbCategory.SelectedItem).ID_Category;

                if (selectedCategoryID == null && selectedSupplierID == null)
                {
                    try
                    {
                        Database.Instance.GetFullDetailProducts();
                    }
                    catch (Exception ex)
                    {
                        SaveWarningHandler(ex, selectedCategoryID, selectedSupplierID);
                    }
                }
                else
                {
                    try
                    {
                        Database.Instance.GetFilteredFullDetailProducts(selectedCategoryID, selectedSupplierID);
                    }
                    catch (Exception ex)
                    {
                        SaveWarningHandler(ex, selectedCategoryID, selectedSupplierID);
                    }
                }
            }
        }

        private void FilterSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbProducts != null)
            {
                var selectedSupplierID = ((SupplierData)filter_cbSupplier.SelectedItem).ID_Supplier;
                var selectedCategoryID = ((CategoryData)filter_cbCategory.SelectedItem).ID_Category;

                if (selectedSupplierID == null && selectedCategoryID == null)
                {
                    try
                    {
                        Database.Instance.GetFullDetailProducts();
                    }
                    catch (Exception ex)
                    {
                        SaveWarningHandler(ex, selectedCategoryID, selectedSupplierID);
                    }
                }
                else
                {
                    try
                    {
                        Database.Instance.GetFilteredFullDetailProducts(selectedCategoryID, selectedSupplierID);
                    }
                    catch (Exception ex)
                    {
                        SaveWarningHandler(ex, selectedCategoryID, selectedSupplierID);
                    }
                }
            }
        }

        private void SaveWarningHandler(Exception ex, int? selectedCategoryID, int? selectedSupplierID)
        {
            MessageBoxResult result = MessageBox.Show($"{ex.Message} Wollen sie diese Speichern? Ansonsten gehen alle änderungen verloren.",
                                                      "Warnung",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Database.Instance.SaveProductList();
                    Database.Instance.GetFilteredFullDetailProducts(selectedCategoryID, selectedSupplierID);
                    break;

                case MessageBoxResult.No:
                    Database.Instance.ClearProductLists();
                    Database.Instance.GetFilteredFullDetailProducts(selectedCategoryID, selectedSupplierID);
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}