using Microsoft.Win32;
using ProductManager.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ProductManager
{
    public partial class ProductsPage : Page
    {
        private MainProductsViewModel vm;

        public ProductsPage()
        {
            InitializeComponent();

            if (this.DataContext as MainProductsViewModel != null)
            {
                vm = this.DataContext as MainProductsViewModel;
            }

            popupMetaData.Closed += Popup_Closed;
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            popupListBox.ItemsSource = null;
            popupAddBtn.Command = null;
            popupRemoveBtn.Command = null;
            popupTextBox.Text = string.Empty;
        }

        private void btnCategorieSettings_Click(object sender, RoutedEventArgs e)
        {
            popupListBox.ItemsSource = vm.CategoryList;
            popupAddBtn.Command = vm.AddCategoryCommand;
            popupRemoveBtn.Command = vm.RemoveCategoryCommand;
            popupListBox.SelectedIndex = 0;

            popupMetaData.IsOpen = popupMetaData.IsOpen == false;
            popupTextBox.Focus();
        }

        private void btnSupplierSettings_Click(object sender, RoutedEventArgs e)
        {
            popupListBox.ItemsSource = vm.SupplierList;
            popupAddBtn.Command = vm.AddSupplierCommand;
            popupRemoveBtn.Command = vm.RemoveSupplierCommand;
            popupListBox.SelectedIndex = 0;

            popupMetaData.IsOpen = popupMetaData.IsOpen == false;
            popupTextBox.Focus();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Multiselect = false;
            openFile.Filter = "Bilddateien (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFile.ShowDialog() == true)
            {
                vm.SetImageCommand.Execute(openFile.FileName);
            }
        }
    }
}