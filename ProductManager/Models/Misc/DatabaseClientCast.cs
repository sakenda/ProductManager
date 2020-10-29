using System;

namespace ProductManager.Models
{
    public static class DatabaseClientCast
    {
        public static T? DBToValue<T>(object value) where T : struct
        {
            if (value != null && value != DBNull.Value)
                return (T)value;
            else
                return null;
        }

        public static object ValueToDb<T>(this object value) where T : struct
        {
            if (value == null)
                return DBNull.Value;

            if (Nullable.GetUnderlyingType(value.GetType()) != null)
            {
                if (!((T?)value).HasValue)
                    return DBNull.Value;
            }

            return (T)value;
        }

        public static object StringToDb(this object value)
        {
            if (value == null || value == DBNull.Value || (Convert.ToString(value)).Length == 0)
                return DBNull.Value;

            return Convert.ToString(value);
        }



        public static T ValidInputCheck<T>(string input)
        {
            // Returns input value as given datatype if input is valid. 
            // If input is invalid -1 will be returned.
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
                    return (T)Convert.ChangeType(-1, typeof(T));
                }
            }
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }


    }
}