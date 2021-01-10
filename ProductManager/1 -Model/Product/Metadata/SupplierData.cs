namespace ProductManager.Model.Product.Metadata
{
    public class SupplierData
    {
        private int? _id;
        private string _name;
        private string _street;
        private string _nr;
        private string _city;
        private string _zip;
        private string _country;

        public int? ID => _id;
        public string Name => _name;
        public string Street => _street;
        public string Nr => _nr;
        public string City => _city;
        public string Zip => _zip;
        public string Country => _country;

        public SupplierData(int? id, string name, string street, string nr, string city, string zip, string country)
        {
            _id = id;
            _name = name;
            _street = street;
            _nr = nr;
            _city = city;
            _zip = zip;
            _country = country;
        }
    }
}