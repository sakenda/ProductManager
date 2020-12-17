namespace ProductManager.Model.Product.Metadata
{
    public class CategoryData
    {
        private int? _id;
        private string _name;

        public int? ID => _id;
        public string Name => _name;

        public CategoryData(int? id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}