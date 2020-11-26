using System.Windows;
using ProductManager.Models.Product;
using ProductManager.ViewModels.DatabaseData;

namespace ProductManager.Views
{
    public partial class NewProductWindow : Window
    {
        //private ProductFullDetail newProduct = new ProductFullDetail();

        public NewProductWindow()
        {
            InitializeComponent();

            //DataContext = newProduct;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //if (BindingGroup.CommitEdit())
            //{
            //    Database.Instance.CurrentProducts.Add(newProduct);
            //    this.Close();
            //}
            if (BindingGroup.CommitEdit())
            {
                Database.Instance.CurrentProducts.Add((ProductFullDetail)TryFindResource("pers"));
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.BindingGroup.CancelEdit();
            this.DialogResult = false;
            this.Close();
        }
    }
}