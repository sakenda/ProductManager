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
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Multiselect = false;
            openFile.Filter = "Bilddateien (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFile.ShowDialog() == true)
            {
                vm.SetImageCommand.Execute(openFile.FileName);
            }
        }
    }
}