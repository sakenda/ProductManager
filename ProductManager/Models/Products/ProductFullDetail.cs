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
        //private int? _CategoryID;
        //private string _CategoryName;
        //private int? _SupplierID;
        //private string _SupplierName;

        public SupplierData SupplierData { get; set; }
        public CategoryData CategoryData { get; set; }

        //public int? SupplierID
        //{
        //    get => _SupplierID;
        //    set
        //    {
        //        if (value != _SupplierID)
        //        {
        //            _SupplierID = value;
        //            OnPropertyChanged(nameof(SupplierID));
        //        }
        //    }
        //}

        //public string SupplierName
        //{
        //    get => _SupplierName;
        //    set
        //    {
        //        if (value != _SupplierName)
        //        {
        //            _SupplierName = value;
        //            OnPropertyChanged(nameof(SupplierName));
        //        }
        //    }
        //}

        //public int? CategoryID
        //{
        //    get => _CategoryID;
        //    set
        //    {
        //        if (value != _CategoryID)
        //        {
        //            _CategoryID = value;
        //            OnPropertyChanged(nameof(CategoryID));
        //        }
        //    }
        //}

        //public string CategoryName
        //{
        //    get => _CategoryName;
        //    set
        //    {
        //        if (value != _CategoryName)
        //        {
        //            _CategoryName = value;
        //            OnPropertyChanged(nameof(CategoryName));
        //        }
        //    }
        //}

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

        public string Error => null;
        public string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Price) && _Price < 0)
                {
                    return "Wert darf nicht Negativ sein";
                }
                if (propertyName == nameof(Quantity) && _Quantity < 0)
                {
                    return "Menge darf nicht Negativ sein";
                }
                return null;
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

    }
}
