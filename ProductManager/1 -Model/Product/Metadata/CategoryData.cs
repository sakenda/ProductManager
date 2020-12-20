namespace ProductManager.Model.Product.Metadata
{
    public class CategoryData
    {
        private int? _id;
        private string _name;
        private string _description;

        public int? ID => _id;
        public string Name => _name;
        public string Description => _description;

        public CategoryData(int? id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;
        }
    }
}