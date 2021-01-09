using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ProductManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Source = new Uri("pack://application:,,,/3 - View/Pages/ProductsPage.xaml");
            tbProducts.IsChecked = true;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                if (tb.Name == "tbProducts")
                {
                    mainFrame.Source = new Uri("pack://application:,,,/3 - View/Pages/ProductsPage.xaml");
                    tbProducts.IsChecked = true;
                    tbUsers.IsChecked = false;
                    tbOrders.IsChecked = false;
                }
                if (tb.Name == "tbUsers")
                {
                    mainFrame.Source = new Uri("pack://application:,,,/3 - View/Pages/UsersPage.xaml");
                    tbProducts.IsChecked = false;
                    tbUsers.IsChecked = true;
                    tbOrders.IsChecked = false;
                }
                if (tb.Name == "tbOrders")
                {
                    mainFrame.Source = new Uri("pack://application:,,,/3 - View/Pages/OrdersPage.xaml");
                    tbProducts.IsChecked = false;
                    tbUsers.IsChecked = false;
                    tbOrders.IsChecked = true;
                }
            }
        }
    }
}