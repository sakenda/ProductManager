using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Model.User.Metadata
{
    public class AdressModel
    {
        private string _street;
        private string _number;
        private string _city;
        private string _zip;
        private string _country;

        public string Street => _street;
        public string Number => _number;
        public string City => _city;
        public string Zip => _zip;
        public string Country => _country;

        public AdressModel()
        {
        }
        public AdressModel(string street, string number, string city, string zip, string country)
        {
            _street = street;
            _number = number;
            _city = city;
            _zip = zip;
            _country = country;
        }
    }
}