using ProductManager.Model.Product;
using System.ComponentModel;

namespace ProductManager.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private Product _product;
        private PString _name;
        private PPrice _price;
        private PInteger _quantity;
        private PString _description;
        private PNullableInteger _categoryId;
        private PNullableInteger _supplierId;
        private bool _changed;
        private bool _needRestock;
        private bool _isEmpty;
        private bool _isDeleted;

        public PString Name => _name;
        public PPrice Price => _price;
        public PInteger Quantity => _quantity;
        public PString Description => _description;
        public PNullableInteger CategoryId { get => _categoryId; set => _categoryId = value; }
        public PNullableInteger SupplierId { get => _supplierId; set => _supplierId = value; }
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }
        public bool NeedRestock
        {
            get => _needRestock;
            set => SetProperty(ref _needRestock, value);
        }
        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set => SetProperty(ref _isDeleted, value);
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

            CheckStock();
        }

        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_name.HasChanged || _description.HasChanged || _quantity.HasChanged || _categoryId.HasChanged || _supplierId.HasChanged || _price.Changed)
            {
                if (_quantity.HasChanged)
                {
                    CheckStock();
                }

                Changed = true;
            }
            else
            {
                Changed = false;
            }
        }

        /// <summary>
        /// Initialisieren der Felder des Objekts.
        /// </summary>
        private void InitializeFields()
        {
            _name = new PString(_product.ProductName);
            _price = new PPrice(_product.Price);
            _quantity = new PInteger(_product.Quantity);
            _description = new PString(_product.Description);
            _categoryId = new PNullableInteger(_product.CategoryID);
            _supplierId = new PNullableInteger(_product.SupplierID);
        }

        /// <summary>
        /// Prüft ob der Mengenwert einen sollwert unterschreitet oder null ist und setzt <see cref="NeedRestock"/> oder <see cref="IsEmpty"/> dementsprechend.
        /// </summary>
        private void CheckStock()
        {
            if (_quantity.Value <= 5 && _quantity.Value >= 1)
            {
                NeedRestock = true;
                IsEmpty = false;
            }

            if (_quantity.Value == 0)
            {
                NeedRestock = false;
                IsEmpty = true;
            }

            if (_quantity.Value > 5)
            {
                NeedRestock = false;
                IsEmpty = false;
            }
        }

        /// <summary>
        /// Zeichnet das aktuelle Objekt aus zum löschen. <see cref="Changed"/> wird auf true gesetzt.
        /// </summary>
        public void DeleteProduct()
        {
            IsDeleted = true;
            Changed = true;
        }

        /// <summary>
        /// Macht alle Änderungen der Daten, die diesen Objekt anhängen, rückgängig.
        /// </summary>
        public void UndoChanges()
        {
            Name.UndoChanges();
            Price.UndoChanges();
            Quantity.UndoChanges();
            Description.UndoChanges();
            CategoryId.UndoChanges();
            SupplierId.UndoChanges();

            IsDeleted = false;

            CheckStock();
        }

        /// <summary>
        /// Speichert alle geänderten Daten, die diesem Objekt anhängen, permanent.
        /// </summary>
        public void AcceptChanges()
        {
            Name.AcceptChanges();
            Price.AcceptChanges();
            Quantity.AcceptChanges();
            Description.AcceptChanges();
            CategoryId.AcceptChanges();
            SupplierId.AcceptChanges();

            CheckStock();
        }

        /// <summary>
        /// Konvertiert das Aktuelle <see cref="ProductViewModel"/> in ein <see cref="Product"/>, zum speichern in die Datenbank.
        /// </summary>
        /// <returns></returns>
        public Product ConvertToProduct()
        {
            Price price = new Price(
                this._product.ProductID,
                this._price.PriceBase.Value,
                this._price.PriceShipping.Value,
                this._price.Profit.Value
                );

            Product p = new Product(
                this.Name.Value,
                price,
                this.Quantity.Value,
                this.Description.Value,
                this.CategoryId.Value,
                this.SupplierId.Value
                );

            p.SetProductID(this._product.ProductID);

            return p;
        }
    }
}