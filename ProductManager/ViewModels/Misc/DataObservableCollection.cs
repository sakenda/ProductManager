using ProductManager.Models;
using System.Collections.ObjectModel;

namespace ProductManager.ViewModels
{
    public class DataObservableCollection<T>
    {
        private ObservableCollection<T> _DataCollection;
        private T _SData;

        public ObservableCollection<T> DataCollection
        {
            get => _DataCollection;
            set => _DataCollection = value;
        }
        public T SData
        {
            get => _SData;
            set => _SData = value;
        }

    }
}
