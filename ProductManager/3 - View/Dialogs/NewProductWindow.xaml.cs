using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using ProductManager.Model.Product;
using ProductManager.ViewModel.DatabaseData;

namespace ProductManager.View
{
    public partial class NewProductWindow : Window
    {
        public NewProductWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            bool areAllValid = true;

            BindingExpression[] bindingFields = new BindingExpression[]
            {
                tbName.GetBindingExpression(TextBox.TextProperty),
                tbPrice.GetBindingExpression(TextBox.TextProperty),
                tbQuantity.GetBindingExpression(TextBox.TextProperty),
                tbDescription.GetBindingExpression(TextBox.TextProperty),
                cbCategory.GetBindingExpression(Selector.SelectedValueProperty),
                cbSupplier.GetBindingExpression(Selector.SelectedValueProperty)
            };

            foreach (var binding in bindingFields)
            {
                if (binding.ValidateWithoutUpdate())
                {
                    continue;
                }
                else
                {
                    areAllValid = false;
                }
            }

            if (!areAllValid)
            {
                return;
            }
            else
            {
                foreach (var binding in bindingFields)
                {
                    binding.UpdateSource();
                }

                Database.Instance.CurrentProducts.Add((ProductFullDetail)TryFindResource("pers"));
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}