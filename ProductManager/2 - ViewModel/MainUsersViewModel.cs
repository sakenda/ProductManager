using ProductManager.Model.User;
using ProductManager.ViewModel.Database;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ProductManager.ViewModel
{
    public class MainUsersViewModel
    {
        private DatabaseUserQueries _database;
        private ObservableCollection<UserModel> _listCollection;
        private ListCollectionView _viewCollection;

        public ListCollectionView ViewCollection => _viewCollection;

        public MainUsersViewModel()
        {
            _database = new DatabaseUserQueries();
            _listCollection = new ObservableCollection<UserModel>();

            _database.GetUsers(ref _listCollection);
            _viewCollection = new ListCollectionView(_listCollection);
        }
    }
}