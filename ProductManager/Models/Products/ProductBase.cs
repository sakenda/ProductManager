using ProductManager.ViewModels;
using System.ComponentModel;

namespace ProductManager.Models
{
    public class ProductBase : INotifyPropertyChanged
    {
        private int _ProductID;
        protected bool _isDirty;

        public int ProductID
        {
            get => _ProductID;
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
        }

        public virtual void SetProductID(int value) => _ProductID = value;
        public virtual void ResetIsDirty() => _isDirty = false;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isDirty) isDirty = true;
        }

    }
}
