using ProductManager.Models.Product.Metadata;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ProductManager.ViewModels.DatabaseData
{
    public class DatabaseMetaData : DatabaseProperties
    {
        public ObservableCollection<CategoryData> CategoryList { get; private set; }
        public ObservableCollection<SupplierData> SupplierList { get; private set; }

        #region Singleton

        private static DatabaseMetaData _instance = null;

        public static DatabaseMetaData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseMetaData();
                }
                return _instance;
            }
        }

        private DatabaseMetaData()
        {
            CategoryList = new ObservableCollection<CategoryData>();
            CategoryList.Add(new CategoryData(null, null));
            GetCategoryFromDB();

            SupplierList = new ObservableCollection<SupplierData>();
            SupplierList.Add(new SupplierData(null, null));
            GetSupplierFromDB();
        }

        #endregion Singleton

        private void GetSupplierFromDB()
        {
            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select s.SupplierID, s.SupplierName "
                            + "from Suppliers s "
                            + "order by s.SupplierID"
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SupplierList.Add(
                            new SupplierData(
                                (int?)reader["SupplierID"],
                                (string)reader["SupplierName"]
                                ));
                    }
                }
            }
        }

        private void GetCategoryFromDB()
        {
            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select c.CategoryID, c.CategoryName "
                            + "from Categories c "
                            + "order by c.CategoryID"
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryList.Add(
                            new CategoryData(
                                (int?)reader["CategoryID"],
                                (string)reader["CategoryName"]
                                ));
                    }
                }
            }
        }
    }
}