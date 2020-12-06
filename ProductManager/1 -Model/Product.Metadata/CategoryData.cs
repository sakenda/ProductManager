namespace ProductManager.Model.Product.Metadata
{
    public class CategoryData
    {
        private string _Name_Category;
        private int? _ID_Category;

        public int? ID_Category
        {
            get { return _ID_Category; }
        }

        public string Name_Category
        {
            get => _Name_Category;
        }

        public CategoryData(int? id, string name)
        {
            _ID_Category = id;
            _Name_Category = name;
        }
    }
}