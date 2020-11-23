using ProductManager.Models.Database;
using ProductManager.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;

namespace ProductManager.Models
{
    public class ProductFullDetail : ProductBase, IDataErrorInfo
    {
        private string _ProductName;
        private string _Description;
        private double _Price;
        private int? _Quantity;

        public SupplierData SupplierData { get; set; }
        public CategoryData CategoryData { get; set; }

        public virtual double Price
        {
            get => _Price;
            set
            {
                if (value != _Price)
                {
                        _Price = value;
                        OnPropertyChanged(nameof(Price));
                }
            }
        }
        public virtual int? Quantity
        {
            get => _Quantity;
            set
            {
                if (value != _Quantity)
                {
                    _Quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public string Description
        {
            get => _Description;
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string ProductName
        {
            get => _ProductName;
            set
            {
                if (value != _ProductName)
                {
                    _ProductName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }

        public ProductFullDetail() : base() { }
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
