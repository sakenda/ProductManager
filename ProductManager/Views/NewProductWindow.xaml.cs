using System;
using System.Windows;
using System.Windows.Input;
using ProductManager.Models;
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
            Product product = new Product();
            product.ProductName = TextBox_ProductName.Text;
            product.Price = Convert.ToDouble(TextBox_ProductPrice.Text);
            product.Quantity = Convert.ToInt32(TextBox_ProductQuantity.Text);
            product.Description = TextBox_ProductDescription.Text;
            product.CategoryID = ((DatabaseMetaData)ComboBox_Category.SelectedItem).DataID;
            product.SupplierID = ((DatabaseMetaData)ComboBox_Supplier.SelectedItem).DataID;

            Database.Instance.ObsCurrentProducts.Add(product);
            this.Close();
        }
        #endregion

        #region Methods
        private void InitializeComboBoxMetaData()
        {
            ComboBox_Category.ItemsSource = MetaData.Instance.CategoryList;
            ComboBox_Category.DisplayMemberPath = nameof(DatabaseMetaData.DataName);
            ComboBox_Category.SelectedValuePath = nameof(DatabaseMetaData.DataID);

            ComboBox_Supplier.ItemsSource = MetaData.Instance.SupplierList;
            ComboBox_Supplier.DisplayMemberPath = nameof(DatabaseMetaData.DataName);
            ComboBox_Supplier.SelectedValuePath = nameof(DatabaseMetaData.DataID);

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
