using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.DatabaseData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductManager.ViewModel
{
    public class MainProductsViewModel : ViewModelBase
    {
        #region "Private Felder"
        private ObservableCollection<ProductViewModel> _listCollection;
        private ListCollectionView _viewCollection;

        private ObservableCollection<CategoryData> _categoryList = DatabaseMetaData.Instance.CategoryList;
        private ObservableCollection<SupplierData> _supplierList = DatabaseMetaData.Instance.SupplierList;

        private CommandBinding _newCommandBinding;
        private CommandBinding _saveCommandBinding;
        private CommandBinding _deleteCommandBinding;

        private string _sortByProperty = _sortCriteria[0];
        private static string[] _sortCriteria = {
            nameof(ProductViewModel.Name),
            nameof(ProductViewModel.Price),
            nameof(ProductViewModel.Quantity),
            nameof(ProductViewModel.CategoryId),
            nameof(ProductViewModel.SupplierId)
        };
        #endregion "Private Felder"

        #region "Öffentliche Eigenschaften"
        public ObservableCollection<ProductViewModel> ListCollection => _listCollection;
        public ListCollectionView ViewCollection => _viewCollection;

        public ObservableCollection<CategoryData> CategoryList => _categoryList;
        public ObservableCollection<SupplierData> SupplierList => _supplierList;

        public CommandBinding NewCommandBinding => _newCommandBinding;
        public CommandBinding SaveCommandBinding => _saveCommandBinding;
        public CommandBinding DeleteCommandBinding => _deleteCommandBinding;

        public string[] SortCriteria => _sortCriteria;
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

        #endregion "Öffentliche Eigenschaften"

        #region "Konstruktor"
        public MainProductsViewModel()
        {
            GetProductsViewModel(ref _listCollection);
            _viewCollection = new ListCollectionView(_listCollection);

            _newCommandBinding = new CommandBinding(ApplicationCommands.New, NewExecuted, NewCanExecute);
            _saveCommandBinding = new CommandBinding(ApplicationCommands.Save, SaveExecuted, SaveCanExecute);
            _deleteCommandBinding = new CommandBinding(ApplicationCommands.Delete, DeleteExecuted, DeleteCanExecute);

            CommandManager.InvalidateRequerySuggested();

            UpdateSorting();
            _viewCollection.MoveCurrentToFirst();
        }
        #endregion "Konstruktor"

        #region "CommandBindings"
        private void NewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ProductViewModel product = new ProductViewModel(null);
            _listCollection.Add(product);
            _viewCollection.MoveCurrentTo(product);
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach (ProductViewModel item in _listCollection)
            {
                if (item.Changed)
                {
                    e.CanExecute = true;
                    return;
                }
                e.CanExecute = false;
            }
        }
        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();

            foreach (ProductViewModel item in _listCollection)
            {
                if (!item.Changed)
                {
                    continue;
                }
                else
                {
                    products.Add(item.ReturnProduct());
                }
            }

            Database.Instance.SaveProductListTest(ref products);
            _viewCollection.MoveCurrentToFirst();
            AcceptChanges();
        }

        private void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewCollection.Count > 0;
        }
        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Product product = _viewCollection.CurrentItem as Product;
            if (product != null)
            {
                Database.Instance.DeletedProducts.Add(product);
                Database.Instance.CurrentProducts.Remove(product);
            }
            _viewCollection.MoveCurrentToFirst();
        }
        #endregion "CommandBindings"

        #region "Private Methoden"
        private void AcceptChanges()
        {
            foreach (ProductViewModel item in _listCollection)
            {
                item.AcceptChanges();
            }
        }

        private void UpdateSorting()
        {
            ViewCollection.SortDescriptions.Clear();
            ViewCollection.SortDescriptions.Add(new SortDescription(this.SortByProperty, ListSortDirection.Ascending));
        }

        private void GetProductsViewModel(ref ObservableCollection<ProductViewModel> liste)
        {
            _listCollection = new ObservableCollection<ProductViewModel>();

            ObservableCollection<Product> temp = Database.Instance.GetProductsTest();

            foreach (Product product in temp)
            {
                liste.Add(new ProductViewModel(product));
            }
        }
        #endregion "Private Methoden"
    }
}