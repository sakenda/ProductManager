using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.DatabaseData;

namespace ProductManager.ViewModel
{
    public class MainProductsViewModel : ViewModelBase
    {
        #region "Private Felder"
        private ListCollectionView _listCollection;

        private CommandBinding _newCommandBinding;
        private CommandBinding _saveCommandBinding;
        private CommandBinding _deleteCommandBinding;

        private string _sortByProperty = _sortCriteria[0];
        private static string[] _sortCriteria = { "ProductName", "Price", "Quantity", "CategoryID", "SupplierID" };

        private ObservableCollection<CategoryData> _categoryList = DatabaseMetaData.Instance.CategoryList;
        private ObservableCollection<SupplierData> _supplierList = DatabaseMetaData.Instance.SupplierList;
        #endregion "Private Felder"

        #region "Öffentliche Eigenschaften"
        public ListCollectionView ListCollection => _listCollection;

        public CommandBinding NewCommandBinding => _newCommandBinding;
        public CommandBinding SaveCommandBinding => _saveCommandBinding;
        public CommandBinding DeleteCommandBinding => _deleteCommandBinding;

        public string SortByProperty
        {
            get { return _sortByProperty; }
            set
            {
                if (value != _sortByProperty)
                {
                    SetProperty(ref _sortByProperty, value);
                    UpdateSorting();
                }
            }
        }

        public string[] SortCriteria
        {
            get { return _sortCriteria; }
        }

        public ObservableCollection<CategoryData> CategoryList => _categoryList;
        public ObservableCollection<SupplierData> SupplierList => _supplierList;
        #endregion "Öffentliche Eigenschaften"

        #region "Konstruktor"
        public MainProductsViewModel()
        {
            _listCollection = CollectionViewSource.GetDefaultView(Database.Instance.CurrentProducts) as ListCollectionView;

            _newCommandBinding = new CommandBinding(ApplicationCommands.New, NewExecuted, NewCanExecute);
            _saveCommandBinding = new CommandBinding(ApplicationCommands.Save, SaveExecuted, SaveCanExecute);
            _deleteCommandBinding = new CommandBinding(ApplicationCommands.Delete, DeleteExecuted, DeleteCanExecute);

            UpdateSorting();
            _listCollection.MoveCurrentToFirst();
        }
        #endregion "Konstruktor"

        #region "CommandBindings"
        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void NewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Database.Instance.SaveProductList();
            _listCollection.MoveCurrentToFirst();
        }
        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach (ProductFullDetail item in Database.Instance.CurrentProducts)
            {
                if (item.isDirty == true)
                {
                    e.CanExecute = true;
                    return;
                }
                e.CanExecute = false;
            }
        }

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ProductFullDetail product = _listCollection.CurrentItem as ProductFullDetail;
            if (product != null)
            {
                Database.Instance.DeletedProducts.Add(product);
                Database.Instance.CurrentProducts.Remove(product);
            }
            _listCollection.MoveCurrentToFirst();
        }
        private void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _listCollection.Count > 0;
        }
        #endregion "CommandBindings"

        #region "Private Methoden"
        private void UpdateSorting()
        {
            _listCollection.SortDescriptions.Clear();
            _listCollection.SortDescriptions.Add(new SortDescription(this.SortByProperty, ListSortDirection.Ascending));
        }

        #endregion "Private Methoden"
    }
}