using ProductManager.Model.Product;
using ProductManager.ViewModel.DatabaseData;

namespace ProductManager.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private int _id;
        private Product _product;
        private PString _name;
        private PDouble _price;
        private PInteger _quantity;
        private PString _description;
        private PNullableInteger _categoryId;
        private PNullableInteger _supplierId;
        private bool _changed;

        public PString Name => _name;
        public PDouble Price => _price;
        public PInteger Quantity => _quantity;
        public PString Description => _description;
        public PNullableInteger CategoryId => _categoryId;
        public PNullableInteger SupplierId => _supplierId;
        public bool Changed
        {
            get { return _changed; }
            set { SetProperty(ref _changed, value); }
        }

        public ProductViewModel(Product product)
        {
            if (product != null)
            {
                _product = product;
                InitializeFields();
            }
            else
            {
                _product = new Product();
                InitializeFields();
                _name.HasChanged = true;
                _price.HasChanged = true;
                _quantity.HasChanged = true;
                _description.HasChanged = true;
                _categoryId.HasChanged = true;
                _supplierId.HasChanged = true;
            }

            _name.PropertyChanged += Product_PropertyChanged;
            _price.PropertyChanged += Product_PropertyChanged;
            _quantity.PropertyChanged += Product_PropertyChanged;
            _description.PropertyChanged += Product_PropertyChanged;
            _categoryId.PropertyChanged += Product_PropertyChanged;
            _supplierId.PropertyChanged += Product_PropertyChanged;
        }

        private void Product_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_name.HasChanged || _description.HasChanged || _price.HasChanged || _quantity.HasChanged || _categoryId.HasChanged || _supplierId.HasChanged)
            {
                Changed = true;
            }
            else
            {
                Changed = false;
            }
        }

        private void InitializeFields()
        {
            _id = _product.ProductID;
            _name = new PString(_product.ProductName);
            _price = new PDouble(_product.Price);
            _quantity = new PInteger(_product.Quantity);
            _description = new PString(_product.Description);
            _categoryId = new PNullableInteger(_product.CategoryID);
            _supplierId = new PNullableInteger(_product.SupplierID);
        }

        public void UndoChanges()
        {
            Name.UndoChanges();
            Price.UndoChanges();
            Quantity.UndoChanges();
            Description.UndoChanges();
            CategoryId.UndoChanges();
            SupplierId.UndoChanges();
        }

        public void AcceptChanges()
        {
            Name.AcceptChanges();
            Price.AcceptChanges();
            Quantity.AcceptChanges();
            Description.AcceptChanges();
            CategoryId.AcceptChanges();
            SupplierId.AcceptChanges();
        }

        public Product ReturnProduct()
        {
            Product p = new Product();

            p.SetProductID(this._id);
            p.ProductName = this.Name.Value;
            p.Price = this.Price.Value;
            p.Quantity = this.Quantity.Value;
            p.Description = this.Description.Value;
            p.CategoryID = this.CategoryId.Value;
            p.SupplierID = this.SupplierId.Value;

            return p;
        }
    }
}