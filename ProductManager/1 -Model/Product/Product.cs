namespace ProductManager.Model.Product
{
    public class Product
    {
        private int _ProductID;
        private string _ProductName;
        private string _Description;
        private Price _price;
        private int _Quantity;
        private int? _CategoryID;
        private int? _SupplierID;

        public int ProductID => _ProductID;
        public string ProductName => _ProductName;
        public string Description => _Description;
        public Price Price => _price;
        public int Quantity => _Quantity;
        public int? CategoryID => _CategoryID;
        public int? SupplierID => _SupplierID;

        public Product()
        {
            _ProductID = -1;
        }

        public Product(string name, Price price, int quantity, string description, int? categoryID, int? supplierID)
        {
            _ProductName = name;
            _price = price;
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