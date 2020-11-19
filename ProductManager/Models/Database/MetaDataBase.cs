using System.ComponentModel;

namespace ProductManager.Models
{
    public class MetaDataBase : ProductFullDetail
    {
        private int? _DataID;

        public int? DataID
        {
            get => _DataID;
            set
            {
                _DataID = value;
                OnPropertyChanged(nameof(DataID));
            }
        }

        public MetaDataBase(int? id)
        {
            DataID = id;
        }
    }
}
