using ProductManager.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ProductManager
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            dgProducts.SelectionChanged += ResetVisibility;
        }

        private void ResetVisibility(object sender, EventArgs e)
        {
            dgProducts.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void ChangeVisibility(object sender, RoutedEventArgs e)
        {
            if (dgProducts.RowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Collapsed)
                dgProducts.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            else
                dgProducts.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }
    }
}