using ProductManager.ViewModels;
using System;

namespace ProductManager.Models
{
    public class Product : ProductBase
    {
        private double _Price;
        private int _Quantity;
        private string _Description;
        private int? _CategoryID;
        private int? _SupplierID;

        public int? SupplierID
        {
            get => _SupplierID;
            set
            {
                if (value != _SupplierID)
                {
                    _SupplierID = value;
                    OnPropertyChanged(nameof(SupplierID));
                }
            }
        }
        public int? CategoryID
        {
            get => _CategoryID;
            set
            {
                if (value != _CategoryID)
                {
                    _CategoryID = value;
                    OnPropertyChanged(nameof(CategoryID));
                }
            }
        }
        public virtual double Price
        {
            get => _Price;
            set
            {
                if (value != _Price)
                {
                    if (value < 0)
                    {
                        InvalidMeasureException ex = new InvalidMeasureException();
                        ex.Data.Add("Time", DateTime.Now);
                        OnInvalidMeasure(new InvalidMeasureEventArgs(value, nameof(Price), ex));
                    }
                    else
                    {
                        _Price = value;
                        OnPropertyChanged(nameof(Price));
                    }
                }
            }
        }
        public virtual int Quantity
        {
            get => _Quantity;
            set
            {
                if (value != _Quantity)
                {
                    if (value < 0)
                    {
                        InvalidMeasureException ex = new InvalidMeasureException();
                        ex.Data.Add("Time", DateTime.Now);
                        OnInvalidMeasure(new InvalidMeasureEventArgs(value, nameof(Quantity), ex));
                    }
                    else
                    {
                        _Quantity = value;
                        OnPropertyChanged(nameof(Quantity));
                    }
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

        public Product() : base() { }
        public Product(string name, double price, int quantity, string description, int? catID, int? supID) : base(name)
        {
            if (price >= 0) _Price = price;
            else
            {
                InvalidMeasureException ex = new InvalidMeasureException();
                ex.Data.Add("Time", DateTime.Now);
                OnInvalidMeasure(new InvalidMeasureEventArgs(price, nameof(Price), ex));
            }

            if (quantity >= 0) _Quantity = quantity;
            else
            {
                InvalidMeasureException ex = new InvalidMeasureException();
                ex.Data.Add("Time", DateTime.Now);
                OnInvalidMeasure(new InvalidMeasureEventArgs(quantity, nameof(Quantity), ex));
            }

            _Description = description;
            _CategoryID = catID;
            _SupplierID = supID;
        }

        protected override void Value_InvalidMeasure(object sender, InvalidMeasureEventArgs e)
        {
            //Console.WriteLine($"Ungültige Wertübergabe bei {e.PropertyName}: {e.InvalidMeasure}. {e.Error.Message}");
        }
    }
}
