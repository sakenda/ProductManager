using ProductManager.Models;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ProductManager.ViewModels
{
    public class MetaData : DatabaseProperties
    {
        public ObservableCollection<DatabaseMetaData> CategoryList { get; private set; }
        public ObservableCollection<DatabaseMetaData> SupplierList { get; private set; }

        #region Singleton
        private static MetaData _instance = null;
        public static MetaData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MetaData();
                }
                return _instance;
            }
        }

        private MetaData()
        {
            CategoryList = new ObservableCollection<DatabaseMetaData>();
            GetProductCategory();

            SupplierList = new ObservableCollection<DatabaseMetaData>();
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
                            new DatabaseMetaData(
                                (int)reader[nameof(ProductMetaData.SupplierID)],
                                reader[nameof(ProductMetaData.SupplierName)].ToString()
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
                            new DatabaseMetaData(
                                (int)reader[nameof(ProductMetaData.CategoryID)],
                                reader[nameof(ProductMetaData.CategoryName)].ToString()
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
