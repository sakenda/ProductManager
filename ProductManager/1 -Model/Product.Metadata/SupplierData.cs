using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Model.Product.Metadata
{
    public class SupplierData
    {
        private string _Name_Supplier;
        private int? _ID_Supplier;

        public int? ID_Supplier
        {
            get { return _ID_Supplier; }
        }

        public string Name_Supplier
        {
            get => _Name_Supplier;
        }

        public SupplierData(int? id, string name)
        {
            _ID_Supplier = id;
            _Name_Supplier = name;
        }
    }
}