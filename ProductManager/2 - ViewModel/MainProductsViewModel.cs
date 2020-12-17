using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.DatabaseData;
using ProductManager.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductManager.ViewModel
{
    public partial class MainProductsViewModel : ViewModelBase
    {
        #region "Private Felder"
        private Database _database;

        private ObservableCollection<ProductViewModel> _listCollection;
        private ObservableCollection<CategoryData> _categoryList;
        private ObservableCollection<SupplierData> _supplierList;
        private ListCollectionView _viewCollection;

        private string _sortByProperty = _sortCriteria[0];
        private static string[] _sortCriteria = {
            "Produktname",
            "Preis",
            "Menge",
            "Kategorie",
            "Hersteller"
        };

        private string _selectedFilter = _filterCriteria[0];
        private static string[] _filterCriteria = {
            "Alle Artikel",
            "Bestand unter Mindestmenge",
            "Kein Bestand"
        };

        private string _searchString;
        #endregion "Private Felder"

        #region "Öffentliche Eigenschaften"
        public ObservableCollection<ProductViewModel> ListCollection => _listCollection;
        public ObservableCollection<CategoryData> CategoryList => _categoryList;
        public ObservableCollection<SupplierData> SupplierList => _supplierList;
        public ListCollectionView ViewCollection => _viewCollection;

        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand NewCommand { get; private set; }

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

        public string[] FilterCriteria => _filterCriteria;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (value != _selectedFilter)
                {
                    SetProperty(ref _selectedFilter, value);
                    UpdateFilter();
                }
            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                SetProperty(ref _searchString, value);
                UpdateSearch();
            }
        }
        #endregion "Öffentliche Eigenschaften"

        #region "Konstruktor"
        public MainProductsViewModel()
        {
            _database = new Database();
            _database.GetSupplier(ref _supplierList);
            _database.GetCategories(ref _categoryList);

            GetProductsViewModel(ref _listCollection);
            _viewCollection = new ListCollectionView(_listCollection);

            DeleteCommand = new RelayCommand(DeleteExecuted, DeleteCanExecute);
            SaveCommand = new RelayCommand(SaveExecuted, SaveCanExecute);
            NewCommand = new RelayCommand(NewExecuted, NewCanExecute);

            UpdateSorting();
            _viewCollection.MoveCurrentToFirst();
        }
        #endregion "Konstruktor"

        #region "Commands"
        private bool NewCanExecute(object sender)
        {
            return true;
        }
        private void NewExecuted(object sender)
        {
            ProductViewModel product = new ProductViewModel(null);
            _listCollection.Add(product);
            _viewCollection.MoveCurrentTo(product);
        }

        private bool SaveCanExecute(object sender)
        {
            foreach (ProductViewModel item in _listCollection)
            {
                if (item.Changed)
                {
                    return true;
                }
            }
            return false;
        }
        private void SaveExecuted(object sender)
        {
            List<Product> changedProducts = new List<Product>();
            List<Product> deletedProducts = new List<Product>();
            List<ProductViewModel> deletedViewList = new List<ProductViewModel>();

            foreach (ProductViewModel item in _listCollection)
            {
                if (!item.Changed)
                {
                    continue;
                }
                else if (item.IsDeleted)
                {
                    deletedProducts.Add(item.ConvertToProduct());
                    deletedViewList.Add(item);
                    continue;
                }
                else
                {
                    changedProducts.Add(item.ConvertToProduct());
                }
            }

            _database.SaveProductList(ref changedProducts, ref deletedProducts);

            if (deletedViewList.Count != 0)
            {
                foreach (ProductViewModel item in deletedViewList)
                {
                    _listCollection.Remove(item);
                }
            }

            if (changedProducts.Count != 0)
            {
                AcceptChanges();
            }

            _viewCollection.Refresh();
            _viewCollection.MoveCurrentToFirst();
        }

        private bool DeleteCanExecute(object sender)
        {
            return ViewCollection.Count > 0;
        }
        private void DeleteExecuted(object sender)
        {
            ProductViewModel product = _viewCollection.CurrentItem as ProductViewModel;
            if (product != null)
            {
                product.DeleteProduct();
            }
        }
        #endregion "Commands"

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
            string property;

            switch (SortByProperty)
            {
                case "Produktname":
                    property = nameof(ProductViewModel.Name);
                    break;
                case "Preis":
                    property = nameof(ProductViewModel.Price);
                    break;
                case "Menge":
                    property = nameof(ProductViewModel.Quantity);
                    break;
                case "Kategorie":
                    property = nameof(ProductViewModel.CategoryId);
                    break;
                case "Hersteller":
                    property = nameof(ProductViewModel.SupplierId);
                    break;
                default:
                    property = null;
                    break;
            }

            ViewCollection.SortDescriptions.Clear();
            ViewCollection.SortDescriptions.Add(new SortDescription(property, ListSortDirection.Ascending));
        }
        private void UpdateFilter()
        {
            _viewCollection.Filter = new Predicate<object>(FilterContains);
        }
        private void UpdateSearch()
        {
            _viewCollection.Filter = new Predicate<object>(SearchContains);
        }

        private bool FilterContains(object obj)
        {
            ProductViewModel product = obj as ProductViewModel;

            if (SelectedFilter.Contains("Alle Artikel")) return true;
            if (SelectedFilter.Contains("Kein Bestand"))
            {
                if (product.Quantity.Value == 0) return true;
                else return false;
            }
            if (SelectedFilter.Contains("Bestand unter Mindestmenge"))
            {
                if (product.Quantity.Value <= 5 && product.Quantity.Value >= 1) return true;
                else return false;
            }

            return true;
        }
        private bool SearchContains(object obj)
        {
            ProductViewModel product = obj as ProductViewModel;

            if (product.Name.Value.ToLower().Contains(_searchString.ToLower())) return true;
            else return false;
        }

        private void GetProductsViewModel(ref ObservableCollection<ProductViewModel> liste)
        {
            _listCollection = new ObservableCollection<ProductViewModel>();

            ObservableCollection<Product> temp = _database.GetProducts();

            foreach (Product product in temp)
            {
                liste.Add(new ProductViewModel(product));
            }
        }
        #endregion "Private Methoden"
    }
}