﻿using ProductManager.Model.Product.Metadata;

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
        public string ProductName => _ProductName;
        public string Description => _Description;
        public PriceModel Price => _price;
        public int Quantity => _Quantity;
        public int? CategoryID => _CategoryID;
        public int? SupplierID => _SupplierID;
        public ImageModel Image => _image;

        public ProductModel()
        {
            ID = -1;
        }

        public ProductModel(string name, PriceModel price, int quantity, string description, int? categoryID, int? supplierID, ImageModel image)
        {
            _ProductName = name;
            _price = price;
            _Quantity = quantity;
            _Description = description;
            _CategoryID = categoryID;
            _SupplierID = supplierID;
            _image = image;
        }

        public void SetID(int value)
        {
            ID = value;
            _price.SetID(value);
            _image.SetID(value);
        }
    }
}