using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductManager.Models
{
    public class ProductBase : PropertyObservable
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
            private set => SetProperty(ref _isDirty, value);
        }

        protected ProductBase()
        {
            _ProductID = -1;
            _isDirty = false;

            PropertyChanged += Value_PropertyChanged;
        }

        public virtual void SetProductID(int value)
        {
            _ProductID = value;
        }

        public virtual void ResetIsDirty()
        {
            _isDirty = false;
        }

        protected virtual void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isDirty)
            {
                isDirty = true;
            }
        }
    }
}