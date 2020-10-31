namespace ProductManager.Models
{
    public class DatabaseMetaData
    {
        public int? DataID { get; set; }
        public string DataName { get; set; }

        public DatabaseMetaData(int? id, string name)
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
