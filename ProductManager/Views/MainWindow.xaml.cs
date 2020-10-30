using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ProductManager.Models;
using ProductManager.ViewModels;

namespace ProductManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ClearFields();
            Database.Instance.LoadProducts();
            DataGrid_ProductList.ItemsSource = Database.Instance.CurrentProducts;
            InitializeComboBoxMetaData();

            _Window.MouseLeftButtonDown += _Window_MouseLeftButtonDown;
            DataGrid_ProductList.SelectionChanged += DataGrid_ProductList_SelectionChanged;
            //DataGrid_ProductList.AutoGeneratingColumn += DataGrid_ProductList_AutoGeneratingColumn;
            ComboBox_Category.SelectionChanged += ComboBox_Category_SelectionChanged;
            ComboBox_Supplier.SelectionChanged += ComboBox_Supplier_SelectionChanged;

            Btn_Save.IsEnabled = true;
            Btn_Delete.IsEnabled = true;
            Btn_AddNew.IsEnabled = true;
            TextBox_ProductID.IsEnabled = false;
        }

        #region Events
        private void _Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void DataGrid_ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_ProductList.SelectedItem is Product selectedProduct)
            {
                TextBox_ProductID.Text = selectedProduct.ProductID.ToString();
                TextBox_ProductName.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.ProductName), selectedProduct));
                TextBox_ProductPrice.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.Price), selectedProduct));
                TextBox_ProductQuantity.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.Quantity), selectedProduct));
                TextBox_ProductDescription.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.Description), selectedProduct));
                ComboBox_Category.SetBinding(Selector.SelectedValueProperty, GetBinding(nameof(selectedProduct.CategoryID), selectedProduct));
                ComboBox_Supplier.SetBinding(Selector.SelectedValueProperty, GetBinding(nameof(selectedProduct.SupplierID), selectedProduct));

                if (selectedProduct.CategoryID.HasValue)
                {
                    ComboBox_Category.SelectedItem = ComboBox_Category.Items.Cast<DatabaseMetaData>().Where(item => item.DataID == selectedProduct.CategoryID).FirstOrDefault();
                }

                if (selectedProduct.SupplierID.HasValue)
                {
                    ComboBox_Supplier.SelectedItem = ComboBox_Supplier.Items.Cast<DatabaseMetaData>().Where(item => item.DataID == selectedProduct.SupplierID).FirstOrDefault();
                }

                RefreshProductList();
            }
        }

        private void ComboBox_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = DataGrid_ProductList.CurrentItem as Product;
            var selectedSupplier = ComboBox_Supplier.SelectedItem as DatabaseMetaData;

            if (selectedItem != null && selectedSupplier != null)
                selectedItem.SupplierID = selectedSupplier.DataID;
            RefreshProductList();
        }

        private void ComboBox_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = DataGrid_ProductList.CurrentItem as Product;
            var selectedCategory = ComboBox_Category.SelectedItem as DatabaseMetaData;

            if (selectedItem != null && selectedCategory != null)
                selectedItem.CategoryID = selectedCategory.DataID;
            RefreshProductList();
        }

        //private void DataGrid_ProductList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    string temp = e.Column.Header.ToString();
        //    switch (temp)
        //    {
        //        case nameof(Product.isDirty):
        //            e.Column.Header = "Not Saved";
        //            e.Column.DisplayIndex = 0;
        //            break;
        //        case nameof(Product.ProductID):
        //            e.Column.Header = "ID";
        //            e.Column.DisplayIndex = 1;
        //            break;
        //        case nameof(Product.ProductName):
        //            e.Column.Header = "Name";
        //            e.Column.DisplayIndex = 2;
        //            break;
        //        case nameof(Product.Price):
        //            e.Column.Header = "Price";
        //            e.Column.DisplayIndex = 3;
        //            break;
        //        case nameof(Product.Quantity):
        //            e.Column.Header = "Quantity";
        //            e.Column.DisplayIndex = 4;
        //            break;
        //        case nameof(Product.Description):
        //            e.Column.Header = "Description";
        //            e.Column.DisplayIndex = 5;
        //            break;
        //        case nameof(ProductMetaData.CategoryID):
        //            e.Cancel = true;
        //            break;
        //        case nameof(ProductMetaData.CategoryName):
        //            e.Column.Header = "Category";
        //            e.Column.DisplayIndex = 6;
        //            break;
        //        case nameof(ProductMetaData.SupplierID):
        //            e.Cancel = true;
        //            break;
        //        case nameof(ProductMetaData.SupplierName):
        //            e.Column.Header = "Supplier";
        //            e.Column.DisplayIndex = 7;
        //            break;
        //        default:
        //            break;
        //    }

        //}
        #endregion

        #region Interaction
        private void Btn_Close_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);
        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid_ProductList.CurrentItem is Product selectedProduct)
            {
                Database.Instance.DeletedProducts.Add(selectedProduct);
                Database.Instance.CurrentProducts.Remove(selectedProduct);
            }
            RefreshProductList();
        }
        private void Btn_AddNew_Click(object sender, RoutedEventArgs e)
        {
            bool? reload;

            NewProductWindow newProductWindow = new NewProductWindow();
            reload = newProductWindow.ShowDialog();

            if (reload.HasValue && reload == true)
            {
                this.RefreshProductList();
            }
        }
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = UserDecisionMessage();
            switch (result)
            {
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    Database.Instance.SaveProductList();
                    Database.Instance.LoadProducts();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Changes are discarded.");
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Methods
        public void RefreshProductList()
        {
            DataGrid_ProductList.ItemsSource = null;
            DataGrid_ProductList.ItemsSource = Database.Instance.CurrentProducts;
        }
        private MessageBoxResult UserDecisionMessage()
        {
            string messageBoxText = "Do you want to save changes?\nPress 'Yes' to save, 'No' to discard changes.";
            string caption = "Save changes";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }
        private void ClearFields()
        {
            TextBox_ProductName.Text = "";
            TextBox_ProductID.Text = "";
            TextBox_ProductPrice.Text = "";
            TextBox_ProductQuantity.Text = "";
            TextBox_ProductDescription.Text = "";
        }
        private Binding GetBinding(string propertyName, Product product)
        {
            return new Binding(propertyName)
            {
                Source = product,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }
        private void InitializeComboBoxMetaData()
        {
            var categoryCollection = new ObservableCollection<DatabaseMetaData>();
            var supplierCollection = new ObservableCollection<DatabaseMetaData>();

            categoryCollection = Database.Instance.GetProductCategory();
            supplierCollection = Database.Instance.GetProductSupplier();



            //ComboBox_Category.ItemsSource = Database.Instance.GetProductCategory();
            //ComboBox_Supplier.ItemsSource = Database.Instance.GetProductSupplier();
        }

        #endregion

    }
}
