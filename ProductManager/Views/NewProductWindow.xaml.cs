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
            Product p;

            /*
            p = new Product(
                TextBox_ProductName.Text.ToString(),
                Convert.ToDouble(TextBox_ProductPrice.Text),
                Convert.ToInt32(TextBox_ProductQuantity.Text),
                TextBox_ProductDescription.Text.ToString(),
                (ComboBox_Categories.SelectedItem as DatabaseMetaData).DataID,
                (ComboBox_Suppliers.SelectedItem as DatabaseMetaData).DataID
            );
            */

            p = new Product();
            p.ProductName = TextBox_ProductName.Text;
            p.Price = Convert.ToDouble(TextBox_ProductPrice.Text);
            p.Quantity = Convert.ToInt32(TextBox_ProductQuantity.Text);
            p.Description = TextBox_ProductDescription.Text;
            p.CategoryID = (ComboBox_Categories.SelectedItem as DatabaseMetaData).DataID;
            p.SupplierID = (ComboBox_Suppliers.SelectedItem as DatabaseMetaData).DataID;

            Database.Instance.CurrentProducts.Add(p);
            this.DialogResult = true;
            this.Close();
        }
        #endregion

        #region Methods
        private void InitializeComboBoxMetaData()
        {
            ComboBox_Categories.ItemsSource = Database.Instance.GetProductCategory();
            ComboBox_Suppliers.ItemsSource = Database.Instance.GetProductSupplier();

            if (ComboBox_Categories.Items.Count > 0)
            {
                ComboBox_Categories.SelectedIndex = 0;
            }

            if (ComboBox_Suppliers.Items.Count > 0)
            {
                ComboBox_Suppliers.SelectedIndex = 0;
            }

        }
        #endregion

    }
}
