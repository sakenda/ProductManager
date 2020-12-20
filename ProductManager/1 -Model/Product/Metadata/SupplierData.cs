namespace ProductManager.Model.Product.Metadata
{
    public class SupplierData
    {
        private int? _id;
        private string _name;
        private string _address;

        public int? ID => _id;
        public string Name => _name;
        public string Address => _address;

        public SupplierData(int? id, string name, string address)
        {
            _id = id;
            _name = name;
            _address = address;
        }
    }
}