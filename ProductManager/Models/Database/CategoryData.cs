using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Models.Database
{
    public class CategoryData : MetaDataBase
    {
        private string _CategoryName;

        public string CategoryName
        {
            get => _CategoryName;
            set
            {
                _CategoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public CategoryData(int? id, string name) : base(id)
        {
            _CategoryName = name;
        }

        public override string ToString()
        {
            return DataID.ToString() + " - " + CategoryName;
        }

    }
}
