namespace ProductManager.Model.Product
{
    public class Product
    {
        private int _ProductID;
        private string _ProductName;
        private string _Description;
        private double _Price;
        private int _Quantity;
        private int? _CategoryID;
        private int? _SupplierID;

        public int ProductID => _ProductID;
        public string ProductName => _ProductName;
        public string Description => _Description;
        public double Price => _Price;
        public int Quantity => _Quantity;
        public int? CategoryID => _CategoryID;
        public int? SupplierID => _SupplierID;

        public Product()
        {
            _ProductID = -1;
        }

        public Product(string name, double price, int quantity, string description, int? categoryID, int? supplierID)
        {
            _ProductName = name;
            _Price = price;
            _Quantity = quantity;
            _Description = description;
            _CategoryID = categoryID;
            _SupplierID = supplierID;
        }

        public virtual void SetProductID(int value)
        {
            _ProductID = value;
        }
    }
}