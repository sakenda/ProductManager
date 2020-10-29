namespace ProductManager.Models
{
    public class DatabaseMetaData
    {
        public int? DataID;
        public string DataName;

        public DatabaseMetaData(int? id, string name)
        {
            DataID = id;
            DataName = name;
        }

        public override string ToString()
        {
            return this.DataName;
        }
    }
}
