using ProductManager.ViewModel.Database;
using ProductManager.ViewModel.Helper;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductManager.ViewModel
{
    public class MainOrdersViewModel : ViewModelBase
    {
        #region "Private Felder"
        private DatabaseUserQueries _database;

        //private ObservableCollection<OrderVM> _listCollection;
        private ListCollectionView _viewCollection;
        private string _sortByProperty = _sortCriteria[0];
        private static string[] _sortCriteria = {
            "Name",
            "Email",
            "Adresse"
        };

        private string _selectedFilter = _filterCriteria[0];
        private static string[] _filterCriteria = {
            "Alle Artikel",
        };

        private string _searchString;
        #endregion "Private Felder"

        #region "Öffentliche Felder"
        //public ObservableCollection<OrderVM> ListCollection => _listCollection;
        public ListCollectionView ViewCollection => _viewCollection;

        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }

        public string[] SortCriteria => _sortCriteria;
        public string SortByProperty
        {
            get { return _sortByProperty; }
            set
            {
                if (value != _sortByProperty)
                {
                    SetProperty(ref _sortByProperty, value);
                    //UpdateSorting();
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
                    //UpdateFilter();
                }
            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                SetProperty(ref _searchString, value);
                //UpdateSearch();
            }
        }
        #endregion "Öffentliche Felder"

        #region "Konstruktor"
        public MainOrdersViewModel()
        {
            _database = new DatabaseUserQueries();

            //GetUserList(ref _listCollection);
            //_viewCollection = new ListCollectionView(_listCollection);

            DeleteCommand = new RelayCommand(DeleteExecuted, DeleteCanExecute);
            SaveCommand = new RelayCommand(SaveExecuted, SaveCanExecute);
            NewCommand = new RelayCommand(NewExecuted);
            UndoCommand = new RelayCommand(UndoExecuted, UndoCanExecute);

            _viewCollection.MoveCurrentToFirst();
        }
        #endregion "Konstruktor"

        #region "Commands"
        private void NewExecuted(object sender)
        {
        }

        private bool SaveCanExecute(object sender)
        {
            return false;
        }
        private void SaveExecuted(object sender)
        {
        }

        private bool DeleteCanExecute(object sender)
        {
            return ViewCollection.Count > 0;
        }
        private void DeleteExecuted(object sender)
        {
        }

        private bool UndoCanExecute(object sender)
        {
            return false;
        }
        private void UndoExecuted(object sender)
        {
        }
        #endregion "Commands"
    }
}