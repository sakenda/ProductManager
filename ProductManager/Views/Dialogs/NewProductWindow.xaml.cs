using System.Windows;
using ProductManager.Models.Product;
using ProductManager.ViewModels.DatabaseData;

namespace ProductManager.Views
{
    public partial class NewProductWindow : Window
    {
        public NewProductWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (grid.BindingGroup.CommitEdit())
            {
                Database.Instance.CurrentProducts.Add((ProductFullDetail)TryFindResource("pers"));
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            grid.BindingGroup.CancelEdit();
            this.DialogResult = false;
            this.Close();
        }
    }
}