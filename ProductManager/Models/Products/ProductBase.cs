using ProductManager.ViewModels;
using System.ComponentModel;

namespace ProductManager.Models
{
    public class ProductBase : INotifyPropertyChanged
    {
        private int _ProductID;
        private string _ProductName;
        protected bool _isDirty;

        public int ProductID { get => _ProductID; }
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
        public bool isDirty
        {
            get => _isDirty;
            private set
            {
                _isDirty = value;
                OnPropertyChanged(nameof(isDirty));
            }
        }

        protected ProductBase()
        {
            _ProductID = -1;
            _isDirty = false;

            PropertyChanged += Value_PropertyChanged;
            InvalidMeasure += Value_InvalidMeasure;
        }

        public ProductBase(string name) : this()
        {
            _ProductName = name;
        }

        public virtual void SetProductID(int value) => _ProductID = value;
        public virtual void ResetIsDirty() => _isDirty = false;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isDirty) isDirty = true;
        }

        public event InvalidMeasureEventHandler InvalidMeasure;
        protected virtual void OnInvalidMeasure(InvalidMeasureEventArgs e)
        {
            if (InvalidMeasure != null) InvalidMeasure(this, e);
            else throw e.Error;
        }
        protected virtual void Value_InvalidMeasure(object sender, InvalidMeasureEventArgs e)
        {
        }
    }
}
