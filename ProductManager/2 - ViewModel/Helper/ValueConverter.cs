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
            return new MainProductsViewModel().CategoryList.Where(p => p.ID_Category == (int?)value).Select(p => p.Name_Category).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().CategoryList.Where(p => p.Name_Category == (string)value).Select(p => p.ID_Category).FirstOrDefault();
        }
    }

    [ValueConversion(typeof(int?), typeof(string))]
    public class SupplierValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().SupplierList.Where(p => p.ID_Supplier == (int?)value).Select(p => p.Name_Supplier).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MainProductsViewModel().SupplierList.Where(p => p.Name_Supplier == (string)value).Select(p => p.ID_Supplier).FirstOrDefault();
        }
    }
}