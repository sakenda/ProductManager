using ProductManager.Models;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ProductManager.ViewModels
{
    public class DatabaseMetaData : DatabaseProperties
    {
        public ObservableCollection<MetaData> CategoryList { get; private set; }
        public ObservableCollection<MetaData> SupplierList { get; private set; }

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
            CategoryList = new ObservableCollection<MetaData>();
            GetProductCategory();

            SupplierList = new ObservableCollection<MetaData>();
            GetProductSupplier();
        }
        #endregion

        public void GetProductSupplier()
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
                            new MetaData(
                                (int?)reader["SupplierID"],
                                (string)reader["SupplierName"]
                                ));
                    }
                }
            }
        }
        public void GetProductCategory()
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
                            new MetaData(
                                (int?)reader["CategoryID"],
                                (string)reader["CategoryName"]
                                ));
                    }
                }
            }
        }



        //public List<DatabaseMetaData> GetProductSupplier()
        //{
        //    List<DatabaseMetaData> list = new List<DatabaseMetaData>();

        //    SqlCommand cmd = new SqlCommand("")
        //    {
        //        CommandText = "select s.SupplierID, s.SupplierName "
        //                    + "from Suppliers s "
        //    };

        //    using (SqlConnection conn = new SqlConnection(DBCONNECTION))
        //    {
        //        cmd.Connection = conn;
        //        conn.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                list.Add(
        //                    new DatabaseMetaData(
        //                        (int)reader[nameof(ProductMetaData.SupplierID)],
        //                        reader[nameof(ProductMetaData.SupplierName)].ToString()
        //                        ));
        //            }
        //        }
        //    }
        //    return list;
        //}

        //public List<DatabaseMetaData> GetProductCategory()
        //{
        //    List<DatabaseMetaData> list = new List<DatabaseMetaData>();

        //    SqlCommand cmd = new SqlCommand("")
        //    {
        //        CommandText = "select c.CategoryID, c.CategoryName "
        //                    + "from Categories c "
        //    };

        //    using (SqlConnection conn = new SqlConnection(DBCONNECTION))
        //    {
        //        cmd.Connection = conn;
        //        conn.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                list.Add(
        //                    new DatabaseMetaData(
        //                        (int)reader[nameof(ProductMetaData.CategoryID)],
        //                        reader[nameof(ProductMetaData.CategoryName)].ToString()
        //                        ));
        //            }
        //        }
        //    }
        //    return list;
        //}

    }
}
