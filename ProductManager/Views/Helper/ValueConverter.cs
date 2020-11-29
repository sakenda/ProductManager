using ProductManager.ViewModels.DatabaseData;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProductManager.Views.Helper
{
    [ValueConversion(typeof(int?), typeof(string))]
    public class CategoryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DatabaseMetaData.Instance.CategoryList.Where(p => p.ID_Category == (int?)value).Select(p => p.Name_Category).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DatabaseMetaData.Instance.CategoryList.Where(p => p.Name_Category == (string)value).Select(p => p.ID_Category).FirstOrDefault();
        }
    }

    [ValueConversion(typeof(int?), typeof(string))]
    public class SupplierValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DatabaseMetaData.Instance.SupplierList.Where(p => p.ID_Supplier == (int?)value).Select(p => p.Name_Supplier).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DatabaseMetaData.Instance.SupplierList.Where(p => p.Name_Supplier == (string)value).Select(p => p.ID_Supplier).FirstOrDefault();
        }
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return ((double)value).ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string amount = value as string;

            if (string.IsNullOrEmpty(amount))
            {
                return null;
            }

            return System.Convert.ToDouble(amount);
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class IntegerValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return ((int)value).ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string amount = value as string;

            if (string.IsNullOrEmpty(amount))
            {
                return null;
            }

            return System.Convert.ToInt32(amount);
        }
    }
}