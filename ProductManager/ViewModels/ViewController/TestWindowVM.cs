using ProductManager.Views;
using System.Windows.Input;

namespace ProductManager.ViewModels
{
    public class TestWindowVM : ViewModelBase
    {
        private readonly DelegateCommand _changeNameCommand;
        public ICommand ChangeNameCommand => _changeNameCommand;

        private string _FirsName;
        public string FirstName
        {
            get { return _FirsName; }
            set { SetProperty(ref _FirsName, value); }
        }

        public TestWindowVM()
        {
            _changeNameCommand = new DelegateCommand(OnChangeName, CanChangeName);
        }

        private void OnChangeName(object commandParameter)
        {
            FirstName = "Walter";
            _changeNameCommand.InvokeCanExecuteChanged();
        }

        private bool CanChangeName(object commandParameter)
        {
            return FirstName != "Walter";
        }

    }
}
