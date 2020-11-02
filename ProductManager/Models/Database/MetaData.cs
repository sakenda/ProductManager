namespace ProductManager.Models
{
    public class MetaData
    {
        public int? DataID { get; set; }
        public string DataName { get; set; }

        public MetaData(int? id, string name)
        {
            DataID = id;
            DataName = name;
        }

        public override string ToString()
        {
            return DataID.ToString() + " - " + DataName;
        }
    }
}
