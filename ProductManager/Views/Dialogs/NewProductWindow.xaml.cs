using System;
using System.Windows;
using ProductManager.Models;
using ProductManager.Models.Database;
using ProductManager.ViewModels;

namespace ProductManager.Views
{
    public partial class NewProductWindow : Window
    {
        private ProductFullDetail newProduct = new ProductFullDetail();

        public NewProductWindow()
        {
            InitializeComponent();
            InitializeComboBoxMetaData();

            DataContext = newProduct;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (BindingGroup.CommitEdit())
            {
                Database.Instance.CurrentProducts.Add(newProduct);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.BindingGroup.CancelEdit();
            this.DialogResult = false;
            this.Close();
        }

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
    }
}