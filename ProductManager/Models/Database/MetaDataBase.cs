using System.ComponentModel;

namespace ProductManager.Models
{
    public class MetaDataBase : ProductFullDetail
    {
        private int? _DataID;

        public int? DataID
        {
            get => _DataID;
            set => SetProperty(ref _DataID, value);
        }

        public MetaDataBase(int? id)
        {
            DataID = id;
        }
    }
}