using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ProductManager.Views.Validation
{
    public class StringValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = value as string;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult(false, "Das Textfeld darf nicht leer sein.");
            }
            else if (text.Length < 3)
            {
                return new ValidationResult(false, "Das Textfeld muss mindestens drei Zeichen haben.");
            }
            return ValidationResult.ValidResult;
        }
    }
}