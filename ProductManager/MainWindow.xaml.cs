using ProductManager.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace ProductManager
{
    public partial class MainWindow : Window
    {
        private Uri productsPage = new Uri("pack://application:,,,/3 - View/Pages/ProductsPage.xaml");
        private Uri usersPage = new Uri("pack://application:,,,/3 - View/Pages/UsersPage.xaml");
        private Uri ordersPage = new Uri("pack://application:,,,/3 - View/Pages/OrdersPage.xaml");

        private (ToggleButton, Uri)[] toggleButtons;

        public MainWindow()
        {
            InitializeComponent();

            toggleButtons = new (ToggleButton, Uri)[] {
                (tbProducts, productsPage),
                (tbUsers, usersPage),
                (tbOrders, ordersPage)
            };

            mainFrame.Source = productsPage;
            tbProducts.IsChecked = true;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                foreach ((ToggleButton, Uri) button in toggleButtons)
                {
                    if (button.Item1.Name == tb.Name)
                    {
                        tb.IsChecked = true;
                        mainFrame.Source = button.Item2;
                        continue;
                    }

                    button.Item1.IsChecked = false;
                }
            }
        }
    }
}