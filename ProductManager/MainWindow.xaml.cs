using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ProductManager
{
    public partial class MainWindow : Window
    {
        private Uri productsPage = new Uri("pack://application:,,,/3 - View/Pages/ProductsPage.xaml");

        private (ToggleButton, Uri)[] toggleButtons;

        public MainWindow()
        {
            InitializeComponent();

            toggleButtons = new (ToggleButton, Uri)[] {
                (tbProducts, productsPage),
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