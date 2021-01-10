using ProductManager.Model.User.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.ViewModel.User.Metadata
{
    public class AdressVM : ViewModelBase
    {
        private AdressModel _adressModel;

        public AdressVM(AdressModel adress)
        {
            if (adress == null)
            {
                _adressModel = new AdressModel(null, null, null, null, null);
                InitializeFields();
            }
            else
            {
                _adressModel = adress;
                InitializeFields();
            }
        }

        private void InitializeFields()
        {
            throw new NotImplementedException();
        }
    }
}