using ProductManager.Models.Database;
using System.ComponentModel;

namespace ProductManager.Models
{
    public class ProductFullDetail : ProductBase, IDataErrorInfo
    {
        private string _ProductName;
        private string _Description;
        private double _Price;
        private int _Quantity;

        private SupplierData _Supplier;
        private CategoryData _Category;

        public SupplierData Supplier
        {
            get => _Supplier;
            set => SetProperty(ref _Supplier, value);
        }

        public CategoryData Category
        {
            get => _Category;
            set => SetProperty(ref _Category, value);
        }

        public virtual double Price
        {
            get => _Price;
            set => SetProperty(ref _Price, value);
        }

        public virtual int Quantity
        {
            get => _Quantity;
            set => SetProperty(ref _Quantity, value);
        }

        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }

        public string ProductName
        {
            get => _ProductName;
            set => SetProperty(ref _ProductName, value);
        }

        public ProductFullDetail() : base()
        {
        }

        public ProductFullDetail(string name, double price, int quantity, string description, CategoryData category, SupplierData supplier) : base()
        {
            _ProductName = name;
            _Price = price;
            _Quantity = quantity;
            _Description = description;
            _Category = category;
            _Supplier = supplier;
        }

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Price))
                {
                    if (_Price < 0)
                    {
                        return "Preis darf nicht Negativ sein";
                    }
                }

                if (propertyName == nameof(Quantity))
                {
                    if (_Price < 0)
                    {
                        return "Menge darf nicht Negativ sein";
                    }
                }

                if (propertyName == nameof(ProductName))
                {
                    if (string.IsNullOrEmpty(_ProductName) || _ProductName.Length < 3)
                    {
                        return "Produktname darf nicht leer oder weniger als drei Zeichen sein.";
                    }
                }

                return null;
            }
        }
    }
}