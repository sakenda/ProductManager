using ProductManager.Model.User;
using ProductManager.ViewModel.Database;
using ProductManager.ViewModel.Helper;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

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
        public ObservableCollection<UserVM> ListCollection => _listCollection;
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
        public MainUsersViewModel()
        {
            _database = new DatabaseUserQueries();

            GetUserList(ref _listCollection);
            _viewCollection = new ListCollectionView(_listCollection);

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
            UserVM user = new UserVM(null);
            _listCollection.Add(user);
            _viewCollection.MoveCurrentTo(user);
        }

        private bool SaveCanExecute(object sender)
        {
            foreach (UserVM item in _listCollection)
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
            throw new NotImplementedException();
        }

        private bool DeleteCanExecute(object sender)
        {
            return ViewCollection.Count > 0;
        }
        private void DeleteExecuted(object sender)
        {
            throw new NotImplementedException();
        }

        private bool UndoCanExecute(object sender)
        {
            if (_viewCollection.CurrentItem != null && ((UserVM)_viewCollection.CurrentItem).Changed)
            {
                return true;
            }
            return false;
        }
        private void UndoExecuted(object sender)
        {
            UserVM user = _viewCollection.CurrentItem as UserVM;
            user.UndoChanges();
        }
        #endregion "Commands"

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