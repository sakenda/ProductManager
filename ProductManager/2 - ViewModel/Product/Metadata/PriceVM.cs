using ProductManager.Model.Product.Metadata;
using System.ComponentModel;

namespace ProductManager.ViewModel.Product.Metadata
{
    public class PriceVM : ViewModelBase
    {
        private const int TAX = 16;
        private const int MWS = 16;

        private PriceModel _priceModel;
        private DecimalVM _priceBase;
        private DecimalVM _priceShipping;
        private DecimalVM _profit;
        private decimal _priceFinal;
        private bool _changed;

        public DecimalVM PriceBase => _priceBase;
        public DecimalVM PriceShipping => _priceShipping;
        public DecimalVM Profit => _profit;
        public decimal PriceFinal
        {
            get => _priceFinal;
            set => SetProperty(ref _priceFinal, value);
        }
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }

        public PriceVM(PriceModel price)
        {
            if (price != null)
            {
                _priceModel = price;
                InitializeFields();
            }
            else
            {
                _priceModel = new PriceModel();
                InitializeFields();
                _priceBase.HasChanged = true;
                _priceShipping.HasChanged = true;
                _profit.HasChanged = true;
            }

            _priceBase.PropertyChanged += Price_PropertyChanged;
            _priceShipping.PropertyChanged += Price_PropertyChanged;
            _profit.PropertyChanged += Price_PropertyChanged;
        }

        /// <summary>
        /// Macht alle Änderungen der Daten, die diesen Objekt anhängen, rückgängig.
        /// </summary>
        public void UndoChanges()
        {
            _priceBase.UndoChanges();
            _priceShipping.UndoChanges();
            _profit.UndoChanges();
        }

        /// <summary>
        /// Speichert alle geänderten Daten, die diesem Objekt anhängen, permanent.
        /// </summary>
        public void AcceptChanges()
        {
            _priceBase.AcceptChanges();
            _priceShipping.AcceptChanges();
            _profit.AcceptChanges();
        }

        /// <summary>
        /// Bei veränderungen einer der Eigenschaften wird <see cref="Changed"/> auf true gesetzt.
        /// <seealso cref="CalculatePrice(PriceVM)"/> wird ausgeführt, zum neu Berechnen der Kosten.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Price_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_priceBase.HasChanged || _priceShipping.HasChanged || _profit.HasChanged)
            {
                this.Changed = true;
                CalculatePrice(this);
            }
            else
            {
                this.Changed = false;
            }
        }

        /// <summary>
        /// Initialisieren der Felder des Price-Objekts. Führt <seealso cref="CalculatePrice(PriceVM)"/> aus.
        /// </summary>
        private void InitializeFields()
        {
            _priceBase = new DecimalVM(_priceModel.BasePrice);
            _priceShipping = new DecimalVM(_priceModel.ShippingPrice);
            _profit = new DecimalVM(_priceModel.Profit);

            CalculatePrice(this);
        }

        /// <summary>
        /// Aktualisiert den Brutto-Preis
        /// </summary>
        /// <param name="price"></param>
        private static void CalculatePrice(PriceVM price)
        {
            decimal result;

            result = price._priceBase.Value + price._priceShipping.Value;
            result += result * (TAX / 100);
            result += result * (price._profit.Value / 100);
            result += result * (MWS / 100);
            price.PriceFinal = result;
        }
    }
}