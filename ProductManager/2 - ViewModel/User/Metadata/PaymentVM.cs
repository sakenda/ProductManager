using ProductManager.Model.User.Metadata;
using System;
using System.ComponentModel;

namespace ProductManager.ViewModel.User.Metadata
{
    public class PaymentVM : ViewModelBase, IViewModel<PaymentModel>
    {
        #region "Private Felder"
        private PaymentModel _paymentModel;

        private StringVM _bic;
        private StringVM _bankName;
        private StringVM _cardtype;
        private bool _changed;
        #endregion "Private Felder"

        #region "Öffentliche Felder"
        public StringVM Bic => _bic;
        public StringVM BankName => _bankName;
        public StringVM CardType => _cardtype;
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }
        #endregion "Öffentliche Felder"

        #region "Konstruktor"
        public PaymentVM(PaymentModel payment)
        {
            if (payment == null)
            {
                _paymentModel = new PaymentModel();
                InitializeFields();
            }
            else
            {
                _paymentModel = payment;
                InitializeFields();
            }

            _bic.PropertyChanged += Payment_PropertyChanged;
            _bankName.PropertyChanged += Payment_PropertyChanged;
            _cardtype.PropertyChanged += Payment_PropertyChanged;
        }
        #endregion "Konstruktor"

        #region "Öffentliche Methoden"
        public void UndoChanges()
        {
            _bic.UndoChanges();
            _bankName.UndoChanges();
            _cardtype.UndoChanges();
        }

        public void AcceptChanges()
        {
            _bic.AcceptChanges();
            _bankName.AcceptChanges();
            _cardtype.AcceptChanges();
        }

        public PaymentModel GetModel()
        {
            return _paymentModel;
        }
        #endregion "Öffentliche Methoden"

        #region "Private Methoden"
        private void Payment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_bic.HasChanged || _bankName.HasChanged || _cardtype.HasChanged)
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
            _bic = new StringVM(_paymentModel.BIC);
            _bankName = new StringVM(_paymentModel.BankName);
            _cardtype = new StringVM(_paymentModel.CartType);
        }
        #endregion "Private Methoden"
    }
}