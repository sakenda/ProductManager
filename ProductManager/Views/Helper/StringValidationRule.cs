using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ProductManager.Views.Helper
{
    public class StringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string invalidChars = @"[(){}*#?!]";
            string text = value as string;

            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult(false, "Das Textfeld darf nicht leer sein.");
            }
            else if (text.Length < 3)
            {
                return new ValidationResult(false, "Das Textfeld muss mindestens drei Zeichen haben.");
            }
            else if (Regex.Matches(text, invalidChars).Count != 0)
            {
                return new ValidationResult(false, "Ungültiges Sonderzeichen.");
            }

            return ValidationResult.ValidResult;
        }
    }
}