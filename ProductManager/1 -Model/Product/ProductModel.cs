using ProductManager.Model.Product.Metadata;

namespace ProductManager.Model.Product
{
    public class ProductModel
    {
        private string _ProductName;
        private string _Description;
        private PriceModel _price;
        private int _Quantity;
        private int? _CategoryID;
        private int? _SupplierID;
        private ImageModel _image;

        public int ID { get; private set; }
        public string ProductName { get => _ProductName; set => _ProductName = value; }
        public string Description { get => _Description; set => _Description = value; }
        public PriceModel Price { get => _price; set => _price = value; }
        public int Quantity { get => _Quantity; set => _Quantity = value; }
        public int? CategoryID { get => _CategoryID; set => _CategoryID = value; }
        public int? SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public ImageModel Image { get => _image; set => _image = value; }

        public ProductModel()
        {
            ID = -1;
            _price = new PriceModel();
            _image = new ImageModel();
        }

        public ProductModel(string name, int quantity, string description, PriceModel price, ImageModel image, int? categoryID, int? supplierID)
        {
            _ProductName = name;
            _Quantity = quantity;
            _Description = description;
            _price = price;
            _image = image;
            _CategoryID = categoryID;
            _SupplierID = supplierID;
        }

        public void SetID(int value)
        {
            ID = value;
        }
    }
}