namespace ProductManager.Models
{
    public class ProductMetaData : Product
    {
        private string _SupplierName;
        private string _CategoryName;

        public string SupplierName
        {
            get => _SupplierName;
            set
            {
                if (_SupplierName != value)
                {
                    _SupplierName = value;
                    OnPropertyChanged(nameof(SupplierName));
                }
            }
        }
        public string CategoryName
        {
            get => _CategoryName;
            set
            {
                if (_CategoryName != value)
                {
                    _CategoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
                }
            }
        }

        public ProductMetaData(string name, double price, int quantity, string description, int? catID, string catName, int? supID, string supName)
            : base(name, price, quantity, description, catID, supID)
        {
            _SupplierName = supName;
            _CategoryName = catName;
        }
    }
}