using System;
using System.Globalization;
using System.Windows.Controls;

namespace ProductManager.View.Helper
{
    public class NumericValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(strValue) || string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(false, $"Keine Eingabe erkannt.");
            }

            switch (ValidationType.Name)
            {
                case "Int32":
                    if (!int.TryParse(strValue, out int intVal))
                    {
                        return new ValidationResult(false, $"Ungültige Zeichen. Nur Zahlen 0-9 erlaubt.");
                    }
                    else
                    {
                        if (intVal < 0)
                        {
                            return new ValidationResult(false, $"Wert darf nicht negativ sein.");
                        }

                        return ValidationResult.ValidResult;
                    }

                case "Double":
                    if (!double.TryParse(strValue, out double doubleVal))
                    {
                        return new ValidationResult(false, $"Ungültige Zeichen. Nur Zahlen 0-9 und Dezimalstellen erlaubt.");
                    }
                    else
                    {
                        if (doubleVal < 0)
                        {
                            return new ValidationResult(false, $"Wert darf nicht negativ sein.");
                        }

                        return ValidationResult.ValidResult;
                    }

                default:
                    throw new InvalidCastException($"{ValidationType.Name} wird nicht unterstützt.");
            }
        }
    }
}