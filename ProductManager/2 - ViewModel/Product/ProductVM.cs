using ProductManager.Model.Product;
using ProductManager.ViewModel.Product.Metadata;
using System.ComponentModel;

namespace ProductManager.ViewModel
{
    public class ProductVM : ViewModelBase, IViewModel<ProductModel>
    {
        #region "Private Felder"
        private ProductModel _product;
        private StringVM _name;
        private PriceVM _price;
        private IntVM _quantity;
        private StringVM _description;
        private ImageVM _image;
        private IntNullVM _categoryId;
        private IntNullVM _supplierId;
        private bool _changed;
        private bool _needRestock;
        private bool _isEmpty;
        private bool _isDeleted;
        #endregion "Private Felder"

        #region "Öffentliche Felder"
        public ProductModel Product => _product;
        public StringVM Name => _name;
        public PriceVM Price => _price;
        public IntVM Quantity => _quantity;
        public StringVM Description => _description;
        public ImageVM Image => _image;
        public IntNullVM CategoryId { get => _categoryId; set => _categoryId = value; }
        public IntNullVM SupplierId { get => _supplierId; set => _supplierId = value; }
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
        #endregion "Öffentliche Felder"

        #region "Konstruktor"
        public ProductVM(ProductModel product)
        {
            if (product != null)
            {
                _product = product;
                InitializeFields();
            }
            else
            {
                _product = new ProductModel();
                InitializeFields();
                _name.HasChanged = true;
                _quantity.HasChanged = true;
                _description.HasChanged = true;
                _categoryId.HasChanged = true;
                _supplierId.HasChanged = true;

                Changed = true;
            }

            _name.PropertyChanged += Product_PropertyChanged;
            _price.PropertyChanged += Product_PropertyChanged;
            _quantity.PropertyChanged += Product_PropertyChanged;
            _description.PropertyChanged += Product_PropertyChanged;
            _categoryId.PropertyChanged += Product_PropertyChanged;
            _supplierId.PropertyChanged += Product_PropertyChanged;
            _image.PropertyChanged += Product_PropertyChanged;

            CheckStock();
        }
        #endregion "Konstruktor"

        #region "Öffentliche Methoden"
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
            Image.UndoChanges();

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
            Image.AcceptChanges();

            _product.ProductName = _name.Value;
            _product.Price.BasePrice = _price.PriceBase.Value;
            _product.Price.ShippingPrice = _price.PriceShipping.Value;
            _product.Price.Profit = _price.Profit.Value;
            _product.Quantity = _quantity.Value;
            _product.Description = _description.Value;
            _product.CategoryID = _categoryId.Value;
            _product.SupplierID = _supplierId.Value;
            _product.Image.FileName = _image.FileName.Value;

            CheckStock();
        }

        /// <summary>
        /// Zeichnet das aktuelle Objekt aus zum löschen. <see cref="Changed"/> wird auf true gesetzt.
        /// </summary>
        public void DeleteProduct()
        {
            _image.RemoveCurrentImage();
            IsDeleted = true;
            Changed = false;
        }

        /// <summary>
        /// Gibt das <see cref="ProductModel"/> zurück.
        /// </summary>
        /// <returns></returns>
        public ProductModel GetModel()
        {
            return _product;
        }
        #endregion "Öffentliche Methoden"

        #region "Private Methoden"
        /// <summary>
        /// Regelt die <see cref="Changed"/> Eigenschaft wenn ein Wert im Objekt geändert wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_name.HasChanged || _description.HasChanged || _quantity.HasChanged || _categoryId.HasChanged || _supplierId.HasChanged || _price.Changed || _image.Changed)
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
            _name = new StringVM(_product.ProductName);
            _price = new PriceVM(_product.Price);
            _quantity = new IntVM(_product.Quantity);
            _description = new StringVM(_product.Description);
            _categoryId = new IntNullVM(_product.CategoryID);
            _supplierId = new IntNullVM(_product.SupplierID);
            _image = new ImageVM(_product.Image);
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
        #endregion "Private Methoden"
    }
}