using System.Globalization;
using System.Windows.Controls;

namespace ProductManager
{
    public class NegativValueRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            if (input != null)
            {
                if (double.TryParse(input, out double val))
                {
                    if (val < 0)
                    {
                        return new ValidationResult(false, "Negativer wert nicht zulässig.");
                    }
                    else
                    {
                        return ValidationResult.ValidResult;
                    }
                }
                else
                {
                    return new ValidationResult(false, "Keine gültige eingabe.");
                }
            }
            else
            {
                return new ValidationResult(false, "Allgemeiner Fehler");
            }
        }
    }
}
