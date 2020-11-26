﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Models.Database
{
    public class SupplierData : MetaDataBase
    {
        private string _SupplierName;

        public string SupplierName
        {
            get => _SupplierName;
            set => SetProperty(ref _SupplierName, value);
        }

        public SupplierData(int? id, string name) : base(id)
        {
            _SupplierName = name;
        }

        public override string ToString()
        {
            return DataID.ToString() + " - " + SupplierName;
        }
    }
}