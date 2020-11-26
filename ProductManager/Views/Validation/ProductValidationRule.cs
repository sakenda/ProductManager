using ProductManager.Models;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductManager.Views.Validation
{
    public class ProductValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string errorMsg = "";
            BindingGroup bg = value as BindingGroup;

            if (bg.Items[0] is ProductFullDetail product)
            {
                string name = (string)bg.GetValue(product, nameof(product.ProductName));
                string price = (string)bg.GetValue(product, nameof(product.Price));
                string quantity = (string)bg.GetValue(product, nameof(product.Quantity));

                if (string.IsNullOrEmpty(name) || name.Length < 3)
                {
                    errorMsg += "\nProduktname darf nicht weniger als drei Zeichen oder leer sein.";
                }

                if (double.TryParse(price, out double convPrice))
                {
                    if (convPrice < 0)
                    {
                        errorMsg += "\nDer Preis darf keinen negativen Wert haben.";
                    }
                }
                else
                {
                    errorMsg += "\nUngültiges Format";
                }

                if (int.TryParse(quantity, out int convQuantity))
                {
                    if (convQuantity < 0)
                    {
                        errorMsg += "\nDie Menge darf keinen negativen Wert haben.";
                    }
                }
                else
                {
                    errorMsg += "\nUngültiges Format";
                }
            }
            else
            {
                return new ValidationResult(false, "\nEs wurde kein Produkt ausgewählt.");
            }

            if (string.IsNullOrEmpty(errorMsg))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, errorMsg);
            }
        }
    }
}