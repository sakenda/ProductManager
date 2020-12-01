using ProductManager.Models.Product;
using ProductManager.ViewModels.DatabaseData;
using ProductManager.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ProductManager
{
    public partial class ProductsPage : Page
    {
        private ListCollectionView view;

        private SortDescription sortByName = new SortDescription(nameof(ProductFullDetail.ProductName), ListSortDirection.Ascending);
        private SortDescription sortByPrice = new SortDescription(nameof(ProductFullDetail.Price), ListSortDirection.Ascending);
        private SortDescription sortByQuantity = new SortDescription(nameof(ProductFullDetail.Quantity), ListSortDirection.Ascending);
        private SortDescription sortByCategory = new SortDescription(nameof(ProductFullDetail.CategoryID), ListSortDirection.Ascending);
        private SortDescription sortBySupplier = new SortDescription(nameof(ProductFullDetail.SupplierID), ListSortDirection.Ascending);

        public ProductsPage()
        {
            InitializeComponent();

            view = CollectionViewSource.GetDefaultView(Database.Instance.CurrentProducts) as ListCollectionView;
            lbProducts.ItemsSource = view;

            view.SortDescriptions.Add(sortByName);

            lbProducts.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;

            CollectionViewSource.GetDefaultView(Database.Instance.CurrentProducts).Refresh();
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

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view.SortDescriptions.Clear();

            string choice = ((ComboBoxItem)cbSort.SelectedItem).Content.ToString();
            switch (choice)
            {
                case "Produktname":
                    view.SortDescriptions.Add(sortByName);
                    break;

                case "Preis":
                    view.SortDescriptions.Add(sortByPrice);
                    break;

                case "Bestand":
                    view.SortDescriptions.Add(sortByQuantity);
                    break;

                case "Kategorie":
                    view.SortDescriptions.Add(sortByCategory);
                    break;

                case "Hersteller":
                    view.SortDescriptions.Add(sortBySupplier);
                    break;
            }
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view.Filter = new Predicate<object>(SetFilter);
        }

        private bool SetFilter(object obj)
        {
            ProductFullDetail product = obj as ProductFullDetail;

            if (((ComboBoxItem)cbFilter.SelectedItem).Content.ToString() == "Alle Artikel") return true;

            switch (((ComboBoxItem)cbFilter.SelectedItem).Content.ToString())
            {
                case "Preis < 3€":
                    return product.Price < 3;

                case "Preis > 3€":
                    return product.Price > 3;

                case "Bestand < 5":
                    return product.Quantity < 5;

                case "Bestand = 0":
                    return product.Quantity == 0;

                default:
                    return false;
            }
        }
    }
}