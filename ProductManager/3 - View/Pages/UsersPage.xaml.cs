using ProductManager.ViewModel;
using System.Windows.Controls;

namespace ProductManager
{
    public partial class UsersPage : Page
    {
        private MainUsersViewModel vm;

        public UsersPage()
        {
            InitializeComponent();

            if (this.DataContext as MainUsersViewModel != null)
            {
                vm = this.DataContext as MainUsersViewModel;
            }
        }
    }
}