namespace ProductManager.Model.User.Metadata
{
    public class PaymentModel
    {
        private string _cardType;
        private string _bic;
        private string _bankName;

        public string CartType => _cardType;
        public string BIC => _bic;
        public string BankName => _bankName;

        public PaymentModel()
        {
        }
        public PaymentModel(string type, string bic, string name)
        {
            _cardType = type;
            _bic = bic;
            _bankName = name;
        }
    }
}