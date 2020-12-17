using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProductManager.ViewModel.Helper
{
    [ValueConversion(typeof(int?), typeof(string))]
    public class CategoryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().CategoryList.Where(p => p.ID == (int?)value).Select(p => p.Name).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().CategoryList.Where(p => p.Name == (string)value).Select(p => p.ID).FirstOrDefault();
        }
    }

    [ValueConversion(typeof(int?), typeof(string))]
    public class SupplierValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().SupplierList.Where(p => p.ID == (int?)value).Select(p => p.Name).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().SupplierList.Where(p => p.Name == (string)value).Select(p => p.ID).FirstOrDefault();
        }
    }
}