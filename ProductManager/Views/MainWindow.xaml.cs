using ProductManager.Models;
using ProductManager.Models.Database;
using ProductManager.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClearFields();
            InitializeComboBoxMetaData();
            Database.Instance.GetFullDetailProducts();

            ListView_ProductList.ItemsSource = Database.Instance.CurrentProducts;

            _Window.MouseLeftButtonDown += _Window_MouseLeftButtonDown;
            ListView_ProductList.SelectionChanged += ListView_ProductList_SelectionChanged;
            ComboBox_Category.SelectionChanged += ComboBox_Category_SelectionChanged;
            ComboBox_Supplier.SelectionChanged += ComboBox_Supplier_SelectionChanged;
        }

        #region Events
        private void _Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void ListView_ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_ProductList.SelectedItem is ProductFullDetail selectedProduct)
            {
                TextBox_ProductID.Text = selectedProduct.ProductID.ToString();

                TextBox_ProductName.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.ProductName), selectedProduct));
                TextBox_ProductPrice.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.Price), selectedProduct));
                TextBox_ProductQuantity.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.Quantity), selectedProduct));
                TextBox_ProductDescription.SetBinding(TextBox.TextProperty, GetBinding(nameof(selectedProduct.Description), selectedProduct));

                ComboBox_Supplier.SelectedValue = selectedProduct.SupplierData.DataID;
                ComboBox_Category.SelectedValue = selectedProduct.CategoryData.DataID;
            }
        }

        private void ComboBox_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ListView_ProductList.SelectedItem as ProductFullDetail;
            var selectedSupplier = ComboBox_Supplier.SelectedItem as SupplierData;

            if (selectedProduct != null && selectedSupplier != null && selectedProduct.SupplierData.DataID != selectedSupplier.DataID)
            {
                selectedProduct.SupplierData.DataID = selectedSupplier.DataID;
                selectedProduct.SupplierData.SupplierName = selectedSupplier.SupplierName;
            }
        }

        private void ComboBox_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ListView_ProductList.SelectedItem as ProductFullDetail;
            var selectedCategory = ComboBox_Category.SelectedItem as CategoryData;

            if (selectedProduct != null && selectedCategory != null && selectedProduct.CategoryData.DataID != selectedCategory.DataID)
            {
                selectedProduct.CategoryData.DataID = selectedCategory.DataID;
                selectedProduct.CategoryData.CategoryName = selectedCategory.CategoryName;
            }
        }
        #endregion

        #region Interaction
        private void Btn_Close_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListView_ProductList.SelectedItem is ProductFullDetail selectedProduct)
            {
                Database.Instance.DeletedProducts.Add(selectedProduct);
                Database.Instance.CurrentProducts.Remove(selectedProduct);
            }
        }

        private void Btn_AddNew_Click(object sender, RoutedEventArgs e)
        {
            NewProductWindow newProductWindow = new NewProductWindow();
            newProductWindow.ShowDialog();
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
                    Database.Instance.GetFullDetailProducts();
                    break;
                case MessageBoxResult.No:
                    Database.Instance.GetFullDetailProducts();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Methods
        private MessageBoxResult UserDecisionMessage()
        {
            string messageBoxText = "Do you want to save changes?\nPress 'Yes' to save and 'No' to revert all changes.";
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

        private Binding GetBinding(string propertyName, ProductFullDetail product)
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
