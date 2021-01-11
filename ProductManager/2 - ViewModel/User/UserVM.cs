using ProductManager.Model.User;
using ProductManager.ViewModel.User.Metadata;
using System.ComponentModel;

namespace ProductManager.ViewModel
{
    public class UserVM : ViewModelBase, IViewModel<UserModel>
    {
        #region "Private Felder"
        private UserModel _userModel;

        private StringVM _firstName;
        private StringVM _lastName;
        private StringVM _email;
        private AdressVM _adress;
        private PaymentVM _payment;
        private bool _changed;
        #endregion "Private Felder"

        #region "Öffentliche Felder"
        public StringVM FirstName => _firstName;
        public StringVM LastName => _lastName;
        public StringVM Email => _email;
        public AdressVM Adress => _adress;
        public PaymentVM Payment => _payment;
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }
        #endregion "Öffentliche Felder"

        #region "Konstruktor"
        public UserVM(UserModel user)
        {
            if (user == null)
            {
                _userModel = new UserModel();
                InitializeFields();
            }
            else
            {
                _userModel = user;
                InitializeFields();
            }

            _firstName.PropertyChanged += User_PropertyChanged;
            _lastName.PropertyChanged += User_PropertyChanged;
            _email.PropertyChanged += User_PropertyChanged;
        }
        #endregion "Konstruktor"

        #region "Öffentliche Methoden"
        public void UndoChanges()
        {
            _firstName.UndoChanges();
            _lastName.UndoChanges();
            _email.UndoChanges();
        }

        public void AcceptChanges()
        {
            _firstName.AcceptChanges();
            _lastName.AcceptChanges();
            _email.AcceptChanges();
        }

        public UserModel GetModel()
        {
            return _userModel;
        }
        #endregion "Öffentliche Methoden"

        #region "Private Methoden"
        private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_firstName.HasChanged || _lastName.HasChanged || _email.HasChanged)
            {
                Changed = true;
            }
            else
            {
                Changed = false;
            }
        }

        private void InitializeFields()
        {
            _firstName = new StringVM(_userModel.UserFirstname);
            _lastName = new StringVM(_userModel.UserLastName);
            _email = new StringVM(_userModel.UserEmail);
            _adress = new AdressVM(_userModel.UserAdress);
            _payment = new PaymentVM(_userModel.UserPayment);
        }
        #endregion "Private Methoden"
    }
}