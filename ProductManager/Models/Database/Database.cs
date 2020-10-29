using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProductManager.Models;

namespace ProductManager.Models
{
    public class Database
    {
        private const string DBCONNECTION = "Server=localhost; Database=TestDB; Trusted_Connection=True;";

        public List<Product> CurrentProducts { get; private set; }
        public List<Product> DeletedProducts { get; private set; }

        #region Singleton
        private static Database _instance = null;
        private Database()
        {
            this.CurrentProducts = new List<Product>();
            this.DeletedProducts = new List<Product>();
        }
        public static Database Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Database();

                return _instance;
            }
        }
        #endregion

        public void LoadProducts()
        {
            Product p;
            this.CurrentProducts.Clear();
            this.DeletedProducts.Clear();

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select p.ProductID, p.ProductName, p.Price, p.Quantity, p.Description, p.CategoryID, "
                            + "c.CategoryName, s.SupplierID, s.SupplierName "
                            + "from Products p "
                            + "left join Categories c on p.CategoryID = c.CategoryID "
                            + "left join Suppliers s on p.SupplierID = s.SupplierID "
                            + "order by p.ProductID"
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p = new Product(
                            reader[nameof(p.ProductName)].ToString(),
                            Convert.ToDouble(reader[nameof(p.Price)]),
                            Convert.ToInt32(reader[nameof(p.Quantity)]),
                            reader[nameof(p.Description)].ToString(),
                            DatabaseClientCast.DBToValue<int>(reader[nameof(p.CategoryID)]),
                            DatabaseClientCast.DBToValue<int>(reader[nameof(p.SupplierID)])
                            );
                        p.SetProductID((int)reader[nameof(p.ProductID)]);

                        CurrentProducts.Add(p);
                    }
                }
            }
        }

        public List<DatabaseMetaData> GetProductSupplier()
        {
            List<DatabaseMetaData> list = new List<DatabaseMetaData>();
            //list.Add(new DatabaseMetaData(null, null));

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select s.SupplierID, s.SupplierName "
                            + "from Suppliers s "
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(
                            new DatabaseMetaData(
                                (int)reader[nameof(ProductMetaData.SupplierID)],
                                reader[nameof(ProductMetaData.SupplierName)].ToString()
                                ));
                    }
                }
            }
            return list;
        }

        public List<DatabaseMetaData> GetProductCategory()
        {
            List<DatabaseMetaData> list = new List<DatabaseMetaData>();
            //list.Add(new DatabaseMetaData(null, null));

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select c.CategoryID, c.CategoryName "
                            + "from Categories c "
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(
                            new DatabaseMetaData(
                                (int)reader[nameof(ProductMetaData.CategoryID)],
                                reader[nameof(ProductMetaData.CategoryName)].ToString()
                                ));
                    }
                }
            }
            return list;
        }

        private void DeleteProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            sql = "delete from Products "
                + "where ProductID = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = product.ProductID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                product.ResetIsDirty();
            }
        }

        private void UpdateProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            sql = "update Products set ProductName = @productName, Price = @price, Quantity = @quantity, Description = @description, "
                + "                        CategoryID = @categoryID, SupplierID = @supplierID "
                + "where ProductID = @id";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = product.ProductID;
            cmd.Parameters.Add("@productName", System.Data.SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
            cmd.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = product.Price;
            cmd.Parameters.Add("@quantity", System.Data.SqlDbType.Int).Value = product.Quantity;
            cmd.Parameters.Add("@description", System.Data.SqlDbType.NVarChar).Value = product.Description.StringToDb();
            cmd.Parameters.Add("@categoryID", System.Data.SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();
            cmd.Parameters.Add("@supplierID", System.Data.SqlDbType.Int).Value = product.SupplierID.ValueToDb<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                product.ResetIsDirty();
            }
        }

        private void InsertProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            sql = "insert into Products(ProductName, Price, Quantity, Description, CategoryID, SupplierID) "
                + "    values (@productName, @price, @quantity, @description, @categoryID, @supplierID)";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@productName", System.Data.SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
            cmd.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = product.Price;
            cmd.Parameters.Add("@quantity", System.Data.SqlDbType.Int).Value = product.Quantity;
            cmd.Parameters.Add("@description", System.Data.SqlDbType.NVarChar).Value = product.Description.StringToDb();
            cmd.Parameters.Add("@categoryID", System.Data.SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();
            cmd.Parameters.Add("@supplierID", System.Data.SqlDbType.Int).Value = product.SupplierID.ValueToDb<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand();
                cmd.CommandText = "select @@IDENTITY";
                cmd.Connection = conn;

                product.SetProductID(Convert.ToInt32(cmd.ExecuteScalar()));
                product.ResetIsDirty();
            }
        }

        public void SaveProductList()
        {
            foreach (Product p in this.DeletedProducts)
            {
                this.DeleteProduct(p);
            }

            foreach (Product p in this.CurrentProducts)
            {
                if (!p.isDirty)
                    continue;

                if (p.ProductID > 0)
                {
                    this.UpdateProduct(p);
                }
                else
                    this.InsertProduct(p);
            }
        }

    }
}
