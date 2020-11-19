using System;
using System.Windows;
using System.Windows.Input;
using ProductManager.Models;
using ProductManager.Models.Database;
using ProductManager.ViewModels;

namespace ProductManager.Views
{
    public partial class NewProductWindow : Window
    {
        public NewProductWindow()
        {
            InitializeComponent();
            InitializeComboBoxMetaData();
            _Window.MouseLeftButtonDown += _Window_MouseLeftButtonDown;
        }

        #region Events
        private void _Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        #endregion

        #region Interaction
        private void Button_CancelCreateProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_CreateProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductFullDetail product = new ProductFullDetail();
            product.ProductName = TextBox_ProductName.Text;
            product.Price = Convert.ToDouble(TextBox_ProductPrice.Text);
            product.Quantity = Convert.ToInt32(TextBox_ProductQuantity.Text);
            product.Description = TextBox_ProductDescription.Text;
            product.CategoryData.DataID = (int?)ComboBox_Category.SelectedValue;
            product.SupplierData.DataID = (int?)ComboBox_Supplier.SelectedValue;

            Database.Instance.CurrentProducts.Add(product);
            this.Close();
        }
        #endregion

        #region Methods
        private void InitializeComboBoxMetaData()
        {
            ComboBox_Category.ItemsSource = DatabaseMetaData.Instance.CategoryList;
            ComboBox_Category.DisplayMemberPath = nameof(CategoryData.CategoryName);
            ComboBox_Category.SelectedValuePath = nameof(CategoryData.DataID);

            ComboBox_Supplier.ItemsSource = DatabaseMetaData.Instance.SupplierList;
            ComboBox_Supplier.DisplayMemberPath = nameof(SupplierData.SupplierName);
            ComboBox_Supplier.SelectedValuePath = nameof(SupplierData.DataID);

            if (ComboBox_Category.Items.Count > 0)
            {
                ComboBox_Category.SelectedIndex = -1;
            }

            if (ComboBox_Supplier.Items.Count > 0)
            {
                ComboBox_Supplier.SelectedIndex = -1;
            }

        }
        #endregion

    }
}
