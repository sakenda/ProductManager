namespace ProductManager.Model.Product.Metadata
{
    public class SupplierData
    {
        private int? _id;
        private string _name;
        private string _address;
        private string _email;

        public int? ID => _id;
        public string Name => _name;
        public string Address => _address;
        public string EMail => _email;

        public SupplierData(int? id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}