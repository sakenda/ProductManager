using ProductManager.Models;
using ProductManager.Models.Database;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ProductManager.ViewModels
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
            GetProductCategory();

            SupplierList = new ObservableCollection<SupplierData>();
            GetProductSupplier();
        }

        #endregion Singleton

        private void GetProductSupplier()
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

        private void GetProductCategory()
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