namespace ProductManager.Model.Product.Metadata
{
    public class PriceModel
    {
        private decimal _basePrice;
        private decimal _shippingPrice;
        private decimal _profit;

        public int ID { get; private set; }
        public decimal BasePrice => _basePrice;
        public decimal ShippingPrice => _shippingPrice;
        public decimal Profit => _profit;

        public PriceModel() { }
        public PriceModel(decimal basePrice, decimal shipping, decimal profit)
        {
            _basePrice = basePrice;
            _shippingPrice = shipping;
            _profit = profit;
        }

        public void SetID(int id)
        {
            ID = id;
        }
    }
}