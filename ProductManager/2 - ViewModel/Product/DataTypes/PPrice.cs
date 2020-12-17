using ProductManager.Model.Product;
using System.ComponentModel;

namespace ProductManager.ViewModel
{
    public class PPrice : ViewModelBase
    {
        private const int TAX = 16;
        private const int MWS = 16;

        private Price _price;
        private PDecimal _priceBase;
        private PDecimal _priceShipping;
        private PDecimal _profit;
        private decimal _priceFinal;
        private bool _changed;

        public PDecimal PriceBase => _priceBase;
        public PDecimal PriceShipping => _priceShipping;
        public PDecimal Profit => _profit;
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

        public PPrice(Price price)
        {
            if (price != null)
            {
                _price = price;
                InitializeFields();
            }
            else
            {
                _price = new Price();
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
        /// Bei veränderungen einer der Eigenschaften wird <see cref="Changed"/> auf true gesetzt.
        /// <seealso cref="CalculatePrice(PPrice)"/> wird ausgeführt, zum neu Berechnen der Kosten.
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
        /// Initialisieren der Felder des Price-Objekts. Führt <seealso cref="CalculatePrice(PPrice)"/> aus.
        /// </summary>
        private void InitializeFields()
        {
            _priceBase = new PDecimal(_price.BasePrice);
            _priceShipping = new PDecimal(_price.ShippingPrice);
            _profit = new PDecimal(_price.Profit);

            CalculatePrice(this);
        }

        /// <summary>
        /// Aktualisiert den Brutto-Preis
        /// </summary>
        /// <param name="price"></param>
        private static void CalculatePrice(PPrice price)
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