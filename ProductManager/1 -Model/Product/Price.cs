namespace ProductManager.Model.Product
{
    public class Price
    {
        private int? _id;
        private decimal _basePrice;
        private decimal _shippingPrice;
        private decimal _profit;

        public int? ID => _id;
        public decimal BasePrice => _basePrice;
        public decimal ShippingPrice => _shippingPrice;
        public decimal Profit => _profit;

        public Price() { }
        public Price(int? id, decimal basePrice, decimal shipping, decimal profit)
        {
            _id = id;
            _basePrice = basePrice;
            _shippingPrice = shipping;
            _profit = profit;
        }

        public void SetID(int id)
        {
            _id = id;
        }
    }
}