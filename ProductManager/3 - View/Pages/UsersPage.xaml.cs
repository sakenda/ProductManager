using ProductManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductManager
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
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