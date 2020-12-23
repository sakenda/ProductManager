namespace ProductManager.Model.Product.Metadata
{
    public class ImageModel
    {
        private string _fileName;

        public int ID { get; private set; }
        public string FileName => _fileName;

        public ImageModel() { }
        public ImageModel(string name)
        {
            _fileName = name;
        }

        public void SetID(int id)
        {
            ID = id;
        }
    }
}