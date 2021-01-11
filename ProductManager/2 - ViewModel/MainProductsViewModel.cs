using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.Database;
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
        private DatabaseProductQueries _database;

        private ObservableCollection<ProductVM> _listCollection;
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
        public ObservableCollection<ProductVM> ListCollection => _listCollection;
        public ObservableCollection<CategoryData> CategoryList => _categoryList;
        public ObservableCollection<SupplierData> SupplierList => _supplierList;
        public ListCollectionView ViewCollection => _viewCollection;

        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand SetImageCommand { get; private set; }
        public ICommand RemoveImageCommand { get; private set; }
        public ICommand AddCategoryCommand { get; private set; }
        public ICommand RemoveCategoryCommand { get; private set; }
        public ICommand AddSupplierCommand { get; private set; }
        public ICommand RemoveSupplierCommand { get; private set; }

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
            _database = new DatabaseProductQueries();
            _database.GetSupplier(ref _supplierList);
            _database.GetCategories(ref _categoryList);

            GetProductsViewModel(ref _listCollection);
            _viewCollection = new ListCollectionView(_listCollection);

            DeleteCommand = new RelayCommand(DeleteExecuted, DeleteCanExecute);
            SaveCommand = new RelayCommand(SaveExecuted, SaveCanExecute);
            NewCommand = new RelayCommand(NewExecuted);
            UndoCommand = new RelayCommand(UndoExecuted, UndoCanExecute);

            SetImageCommand = new RelayCommand(SetImageExecuted);
            RemoveImageCommand = new RelayCommand(RemoveImageExecuted, RemoveImageCanExecute);

            AddCategoryCommand = new RelayCommand(AddCategoryExecuted);
            RemoveCategoryCommand = new RelayCommand(RemoveCategoryExecuted, RemoveCategoryCanExecute);
            AddSupplierCommand = new RelayCommand(AddSupplierExecuted);
            RemoveSupplierCommand = new RelayCommand(RemoveSupplierExecuted, RemoveSupplierCanExecute);

            UpdateSorting();
            _viewCollection.MoveCurrentToFirst();
        }

        #endregion "Konstruktor"

        #region "Commands"
        private void NewExecuted(object sender)
        {
            ProductVM product = new ProductVM(null);
            _listCollection.Add(product);
            _viewCollection.MoveCurrentTo(product);
        }

        private bool SaveCanExecute(object sender)
        {
            foreach (ProductVM item in _listCollection)
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
            List<ProductModel> changedProducts = new List<ProductModel>();
            List<ProductModel> deletedProducts = new List<ProductModel>();
            List<ProductVM> deletedViewList = new List<ProductVM>();

            foreach (ProductVM item in _listCollection)
            {
                if (item.IsDeleted)
                {
                    item.AcceptChanges();
                    deletedProducts.Add(item.GetModel());
                    deletedViewList.Add(item);
                    continue;
                }
                else if (item.Changed)
                {
                    item.AcceptChanges();
                    changedProducts.Add(item.GetModel());
                }
                else
                {
                    continue;
                }
            }

            if (deletedViewList.Count != 0)
            {
                foreach (ProductVM item in deletedViewList)
                {
                    _listCollection.Remove(item);
                }
            }

            _database.SaveProductList(ref changedProducts, ref deletedProducts);
            GetProductsViewModel(ref _listCollection);

            _viewCollection.Refresh();
            _viewCollection.MoveCurrentToFirst();
        }

        private bool DeleteCanExecute(object sender)
        {
            return ViewCollection.Count > 0;
        }
        private void DeleteExecuted(object sender)
        {
            ProductVM product = _viewCollection.CurrentItem as ProductVM;
            if (product != null)
            {
                if (product.Product.ID == -1)
                {
                    _listCollection.Remove(product);
                }
                else
                {
                    product.DeleteProduct();
                }
            }
        }

        private bool UndoCanExecute(object sender)
        {
            if (_viewCollection.CurrentItem != null && ((ProductVM)_viewCollection.CurrentItem).Changed)
            {
                return true;
            }
            return false;
        }
        private void UndoExecuted(object sender)
        {
            ProductVM product = (ProductVM)_viewCollection.CurrentItem;
            product.UndoChanges();
        }

        private void SetImageExecuted(object obj)
        {
            (string FilePath, string FileName) imageFile = ((string, string))obj;

            ProductVM product = _viewCollection.CurrentItem as ProductVM;

            if (imageFile.FilePath != null && imageFile.FileName != null && product != null)
            {
                product.Image.ChangeImage(imageFile.FileName, imageFile.FilePath);
            }
        }

        private bool RemoveImageCanExecute(object obj)
        {
            if (_viewCollection.CurrentItem != null)
            {
                ProductVM item = (ProductVM)_viewCollection.CurrentItem;

                if (item != null && !string.IsNullOrEmpty(item.Image.FileName.Value))
                {
                    return true;
                }
            }
            return false;
        }
        private void RemoveImageExecuted(object obj)
        {
            ProductVM product = _viewCollection.CurrentItem as ProductVM;
            product.Image.RemoveCurrentImage();
        }

        private void AddCategoryExecuted(object obj)
        {
            CategoryData data = new CategoryData(null, obj as string, null);
            CategoryList.Add(data);
            _database.InsertCategory(data);
        }
        private bool RemoveCategoryCanExecute(object obj)
        {
            if (obj as CategoryData != null)
            {
                return true;
            }
            return false;
        }
        private void RemoveCategoryExecuted(object obj)
        {
            var item = obj as CategoryData;
            CategoryList.Remove(item);
            _database.DeleteCategory(item);
        }

        private void AddSupplierExecuted(object obj)
        {
            SupplierData data = new SupplierData(null, obj as string, null, null, null, null, null);
            SupplierList.Add(data);
            _database.InsertSupplier(data);
        }
        private bool RemoveSupplierCanExecute(object obj)
        {
            if (obj as SupplierData != null)
            {
                return true;
            }
            return false;
        }
        private void RemoveSupplierExecuted(object obj)
        {
            var item = obj as SupplierData;
            SupplierList.Remove(item);
            _database.DeleteSupplier(item);
        }
        #endregion "Commands"

        #region "Private Methoden"
        private void UpdateSorting()
        {
            string property;

            switch (SortByProperty)
            {
                case "Produktname":
                    property = nameof(ProductVM.Name);
                    break;
                case "Preis":
                    property = nameof(ProductVM.Price.PriceFinal);
                    break;
                case "Menge":
                    property = nameof(ProductVM.Quantity);
                    break;
                case "Kategorie":
                    property = nameof(ProductVM.CategoryId);
                    break;
                case "Hersteller":
                    property = nameof(ProductVM.SupplierId);
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
            ProductVM product = obj as ProductVM;

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
            ProductVM product = obj as ProductVM;

            if (product.Name.Value.ToLower().Contains(_searchString.ToLower())) return true;
            else return false;
        }

        private void GetProductsViewModel(ref ObservableCollection<ProductVM> liste)
        {
            _listCollection = new ObservableCollection<ProductVM>();

            ObservableCollection<ProductModel> temp = _database.GetProducts();

            foreach (ProductModel product in temp)
            {
                liste.Add(new ProductVM(product));
            }
        }
        #endregion "Private Methoden"
    }
}