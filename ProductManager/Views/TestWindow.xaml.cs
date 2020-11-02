using ProductManager.ViewModels;
using System.Windows;

namespace ProductManager.Views
{
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            var viewModel = new TestWindowVM();
            DataContext = viewModel;

        }
    }
}
