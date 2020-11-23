using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ProductManager.Models;
using ProductManager.Models.Database;

namespace ProductManager.Views.Validation
{
    public class ProductValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            BindingGroup bindingGroup = value as BindingGroup;
            if (bindingGroup.Items.Count == 1)
            {
                string name;
                double? price;
                int? quantity;
                SupplierData supplier;

                ProductFullDetail product = (ProductFullDetail)bindingGroup.Items[0];
                name = bindingGroup.GetValue(product, nameof(product.ProductName)) as string;
                price = bindingGroup.GetValue(product, nameof(product.Price)) as double?;
                quantity = bindingGroup.GetValue(product, nameof(product.Quantity)) as int?;
                supplier = bindingGroup.GetValue(product, nameof(product.SupplierData)) as SupplierData;

                if (string.IsNullOrEmpty(name) || name.Length < 3)
                {
                    return new ValidationResult(false, "Produktname darf nicht weniger als drei Zeichen oder leer sein.");
                }

                if (price < 0)
                {
                    return new ValidationResult(false, "Der Preis darf keinen negativen Wert haben.");
                }

                if (quantity < 0)
                {
                    return new ValidationResult(false, "Die Menge darf keinen negativen Wert haben.");
                }

                if (supplier == null)
                {
                    return new ValidationResult(false, "Es muss ein Hersteller angegeben werden.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
