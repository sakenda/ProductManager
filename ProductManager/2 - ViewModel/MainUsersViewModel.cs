using ProductManager.Model.User;
using ProductManager.ViewModel.Database;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ProductManager.ViewModel
{
    public class MainUsersViewModel : ViewModelBase
    {
        #region "Private Felder"
        private DatabaseUserQueries _database;

        private ObservableCollection<UserVM> _listCollection;
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

        #region "Öffentliche Felder"
        public ObservableCollection<UserVM> ListCollection => _listCollection;
        public ListCollectionView ViewCollection => _viewCollection;

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
        public MainUsersViewModel()
        {
            _database = new DatabaseUserQueries();

            GetUserList(ref _listCollection);
            _viewCollection = new ListCollectionView(_listCollection);

            _viewCollection.MoveCurrentToFirst();
        }
        #endregion "Konstruktor"

        #region "Private Methoden"
        private void GetUserList(ref ObservableCollection<UserVM> list)
        {
            list = new ObservableCollection<UserVM>();
            ObservableCollection<UserModel> temp = _database.GetUsers();

            foreach (UserModel user in temp)
            {
                list.Add(new UserVM(user));
            }
        }
        #endregion "Private Methoden"
    }
}