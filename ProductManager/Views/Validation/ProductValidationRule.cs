using ProductManager.Models.Product;
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

                if (!string.IsNullOrEmpty(price) && double.TryParse(price, out double priceValue))
                {
                    if (priceValue < 0)
                    {
                        errorMsg += "\nDer Preis darf keinen negativen Wert haben.";
                    }
                }
                else
                {
                    errorMsg += "\nPreis leer oder ungültige Zeichenkette";
                }

                if (!string.IsNullOrEmpty(quantity) && int.TryParse(quantity, out int quantityValue))
                {
                    if (quantityValue < 0)
                    {
                        errorMsg += "\nDie Menge darf keinen negativen Wert haben.";
                    }
                }
                else
                {
                    errorMsg += "\nMenge leer oder ungültige Zeichenkette";
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