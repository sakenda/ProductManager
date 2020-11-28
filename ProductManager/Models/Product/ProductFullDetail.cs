﻿using System.ComponentModel;

namespace ProductManager.Models.Product
{
    public class ProductFullDetail : ProductBase, IDataErrorInfo
    {
        private string _ProductName;
        private string _Description;
        private double _Price;
        private int _Quantity;
        private bool _OutOfStock;
        private bool _RestockThreshold;

        private int? _CategoryID;
        private int? _SupplierID;

        public int? SupplierID
        {
            get => _SupplierID;
            set => SetProperty(ref _SupplierID, value);
        }

        public int? CategoryID
        {
            get => _CategoryID;
            set => SetProperty(ref _CategoryID, value);
        }

        public virtual double Price
        {
            get => _Price;
            set => SetProperty(ref _Price, value);
        }

        public virtual int Quantity
        {
            get => _Quantity;
            set
            {
                SetProperty(ref _Quantity, value);
                RestockThreshold = Quantity <= 5 && Quantity >= 1;
                OutOfStock = Quantity == 0;
            }
        }

        public bool RestockThreshold
        {
            get => _RestockThreshold;
            private set => SetProperty(ref _RestockThreshold, value);
        }

        public bool OutOfStock
        {
            get => _OutOfStock;
            private set => SetProperty(ref _OutOfStock, value);
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

        public ProductFullDetail(string name, double price, int quantity, string description, int? categoryID, int? supplierID) : base()
        {
            _ProductName = name;
            _Price = price;
            _Quantity = quantity;
            _Description = description;
            _CategoryID = categoryID;
            _SupplierID = supplierID;
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