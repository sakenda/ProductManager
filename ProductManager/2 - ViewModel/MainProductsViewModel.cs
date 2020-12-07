using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.DatabaseData;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

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
            _listCollection = new ListCollectionView(Database.Instance.CurrentProducts);

            _newCommandBinding = new CommandBinding(ApplicationCommands.New, NewExecuted, NewCanExecute);
            _saveCommandBinding = new CommandBinding(ApplicationCommands.Save, SaveExecuted, SaveCanExecute);
            _deleteCommandBinding = new CommandBinding(ApplicationCommands.Delete, DeleteExecuted, DeleteCanExecute);

            _listCollection.CurrentChanging += _listCollection_CurrentChanging;

            UpdateSorting();
            _listCollection.MoveCurrentToFirst();
        }

        private void _listCollection_CurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            if (((Product)_listCollection.CurrentItem).isDirty)
            {
            }
        }
        #endregion "Konstruktor"

        #region "CommandBindings"
        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Product product = new Product();
            _listCollection.AddNewItem(product);
            _listCollection.MoveCurrentTo(product);
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
            foreach (Product item in Database.Instance.CurrentProducts)
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
            Product product = _listCollection.CurrentItem as Product;
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