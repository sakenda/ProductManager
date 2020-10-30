using System;

namespace ProductManager.ViewModels
{
    public static class Validation
    {
        public static T ValidInputCheck<T>(string input)
        {
            // Returns input value as given datatype if input is valid. 
            // If input is invalid null will be returned.
            // If Comma is present in input it will be flaged as double.
            // A second comma is not allowed

            string stringValue = null;
            bool isDouble = false;

            foreach (char c in input)
            {
                if (Char.IsDigit(c))
                {
                    stringValue += c;
                    continue;
                }
                else if (c.ToString() == "," && !isDouble)
                {
                    stringValue += c;
                    isDouble = true;
                    continue;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Ungültiges Format. Drücke eine Taste...");
                    Console.ReadKey();
                    return (T)Convert.ChangeType(null, typeof(T));
                }
            }
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
    }
}
