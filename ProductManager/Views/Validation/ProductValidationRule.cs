using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ProductManager.Models;

namespace ProductManager.Views.Validation
{
    public class ProductValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string errorMsg = "";

            BindingGroup bg = value as BindingGroup;

            if (bg.Items.Count == 1)
            {
                ProductFullDetail product = (ProductFullDetail)bg.Items[0];

                string name = (string)bg.GetValue(product, nameof(product.ProductName));
                double? price = (double?)bg.GetValue(product, nameof(product.Price));
                int? quantity = (int?)bg.GetValue(product, nameof(product.Quantity));

                if (string.IsNullOrEmpty(name) || name.Length < 3)
                {
                    errorMsg += "Produktname darf nicht weniger als drei Zeichen oder leer sein.\n";
                }

                if (price < 0 || price == null)
                {
                    errorMsg += "Der Preis darf keinen negativen Wert haben.\n";
                }

                if (quantity < 0 || quantity == null)
                {
                    errorMsg += "Die Menge darf keinen negativen Wert haben.";
                }
            }
            else
            {
                errorMsg += "Es wurden keine Daten übergeben.";
            }

            return string.IsNullOrEmpty(errorMsg) ? ValidationResult.ValidResult : new ValidationResult(false, errorMsg);
        }
    }
}