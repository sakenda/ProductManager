using ProductManager.Models.Database;
using System.ComponentModel;

namespace ProductManager.Models
{
    public class ProductFullDetail : ProductBase, IDataErrorInfo
    {
        private string _ProductName;
        private string _Description;
        private double _Price;
        private int? _Quantity;

        public SupplierData SupplierData
        {
            get;
            set;
        }

        public CategoryData CategoryData
        {
            get;
            set;
        }

        public virtual double Price
        {
            get => _Price;
            set
            {
                _Price = value;
                OnPropertyChanged();
            }
        }

        public virtual int? Quantity
        {
            get => _Quantity;
            set
            {
                _Quantity = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged();
            }
        }

        public string ProductName
        {
            get => _ProductName;
            set
            {
                _ProductName = value;
                OnPropertyChanged();
            }
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
            CategoryData = category;
            SupplierData = supplier;
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
                        return "INDEX: Preis darf nicht Negativ sein";
                    }
                }

                if (propertyName == nameof(Quantity))
                {
                    if (_Price < 0)
                    {
                        return "INDEX: Menge darf nicht Negativ sein";
                    }
                }

                if (propertyName == nameof(ProductName))
                {
                    if (string.IsNullOrEmpty(_ProductName) || _ProductName.Length < 3)
                    {
                        return "INDEX: Produktname darf nicht leer oder weniger als drei Zeichen sein.";
                    }
                }

                return null;
            }
        }
    }
}