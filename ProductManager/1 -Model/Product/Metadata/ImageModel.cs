namespace ProductManager.Model.Product.Metadata
{
    public class ImageModel
    {
        private string _path;

        public int ID { get; private set; }
        public string Path => _path;

        public ImageModel() { }
        public ImageModel(string path)
        {
            _path = path;
        }

        public void SetID(int id)
        {
            ID = id;
        }
    }
}