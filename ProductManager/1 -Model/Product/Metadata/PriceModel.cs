namespace ProductManager.Model.Product.Metadata
{
    public class PriceModel
    {
        private decimal _basePrice;
        private decimal _shippingPrice;
        private decimal _profit;

        public decimal BasePrice { get => _basePrice; set => _basePrice = value; }
        public decimal ShippingPrice { get => _shippingPrice; set => _shippingPrice = value; }
        public decimal Profit { get => _profit; set => _profit = value; }

        public PriceModel()
        {
        }
        public PriceModel(decimal basePrice, decimal shipping, decimal profit)
        {
            _basePrice = basePrice;
            _shippingPrice = shipping;
            _profit = profit;
        }
    }
}