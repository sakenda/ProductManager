using ProductManager.Model.User.Metadata;

namespace ProductManager.Model.User
{
    public class UserModel
    {
        private int? _userID;

        private string _userFirstName;
        private string _userLastName;
        private string _userEmail;
        private AdressModel _userAdress;
        private PaymentModel _userPayment;

        public string UserFirstname => _userFirstName;
        public string UserLastName => _userLastName;
        public string UserEmail => _userEmail;
        public AdressModel UserAdress => _userAdress;
        public PaymentModel UserPayment => _userPayment;

        public UserModel()
        {
            _userID = -1;
        }
        public UserModel(string firstNamem, string lastname, string email, AdressModel adress, PaymentModel payment) : this()
        {
            _userFirstName = firstNamem;
            _userLastName = lastname;
            _userEmail = email;
            _userAdress = adress;
            _userPayment = payment;
        }

        public void SetID(int? id)
        {
            _userID = id;
        }
    }
}