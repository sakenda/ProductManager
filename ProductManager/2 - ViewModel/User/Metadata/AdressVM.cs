using ProductManager.Model.User.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProductManager.ViewModel.User.Metadata
{
    public class AdressVM : ViewModelBase, IViewModel<AdressModel>
    {
        #region "Private Felder"
        private AdressModel _adressModel;
        private StringVM _street;
        private StringVM _number;
        private StringVM _city;
        private StringVM _zip;
        private StringVM _country;
        private bool _changed;
        #endregion "Private Felder"

        #region "Öffentliche Felder"
        public StringVM Street => _street;
        public StringVM Number => _number;
        public StringVM City => _city;
        public StringVM Zip => _zip;
        public StringVM Country => _country;
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }
        #endregion "Öffentliche Felder"

        #region "Konstruktor"
        public AdressVM(AdressModel adress)
        {
            if (adress == null)
            {
                _adressModel = new AdressModel();
                InitializeFields();
            }
            else
            {
                _adressModel = adress;
                InitializeFields();
            }

            _street.PropertyChanged += AdressVM_PropertyChanged;
        }
        #endregion "Konstruktor"

        #region "Öffentliche Methoden"
        public void UndoChanges()
        {
            _street.UndoChanges();
            _number.UndoChanges();
            _city.UndoChanges();
            _zip.UndoChanges();
            _country.UndoChanges();
        }

        public void AcceptChanges()
        {
            _street.AcceptChanges();
            _number.AcceptChanges();
            _city.AcceptChanges();
            _zip.AcceptChanges();
            _country.AcceptChanges();
        }

        public AdressModel GetModel()
        {
            return _adressModel;
        }
        #endregion "Öffentliche Methoden"

        #region "Private Methoden"
        private void InitializeFields()
        {
            _street = new StringVM(_adressModel.Street);
            _number = new StringVM(_adressModel.Number);
            _city = new StringVM(_adressModel.City);
            _zip = new StringVM(_adressModel.Zip);
            _country = new StringVM(_adressModel.Country);
        }

        private void AdressVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_street.HasChanged || _number.HasChanged || _city.HasChanged || _zip.HasChanged || _country.HasChanged)
            {
                Changed = true;
            }
            else
            {
                Changed = false;
            }
        }
        #endregion "Private Methoden"
    }
}