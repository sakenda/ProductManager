using ProductManager.Model.User.Metadata;
using System;

namespace ProductManager.ViewModel.User.Metadata
{
    public class PaymentVM : ViewModelBase
    {
        private PaymentModel _paymentModel;

        public PaymentVM(PaymentModel payment)
        {
            if (payment == null)
            {
                _paymentModel = new PaymentModel(null, null, null);
                InitializeFields();
            }
            else
            {
                _paymentModel = payment;
                InitializeFields();
            }
        }

        private void InitializeFields()
        {
            throw new NotImplementedException();
        }
    }
}