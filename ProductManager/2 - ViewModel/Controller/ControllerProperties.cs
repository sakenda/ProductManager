using System.IO;

namespace ProductManager.ViewModel.Controller
{
    public static class Properties
    {
        public static readonly string IMAGE_PATH = Directory.GetCurrentDirectory() + @"\Images\";
        public static readonly string SETTINGS_PATH = Directory.GetCurrentDirectory() + @"\Settings\";
        public static readonly string PRODUCTS_PATH = Directory.GetCurrentDirectory() + @"\Products\";
    }
}