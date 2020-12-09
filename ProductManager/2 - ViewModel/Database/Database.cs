using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace ProductManager.ViewModel.DatabaseData
{
    public class Database : DatabaseProperties
    {
        public ObservableCollection<Product> GetProducts()
        {
            ObservableCollection<Product> list = new ObservableCollection<Product>();
            Product product;

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select p.product_id, "
                            + "p.product_name, "
                            + "p.product_price, "
                            + "p.product_quantity, "
                            + "p.product_description, "
                            + "p.product_category_id, "
                            + "p.product_supplier_id, "
                            + "c.category_name, "
                            + "s.supplier_name "
                            + "from Products p "
                            + "left join categories c on p.product_category_id = c.category_id "
                            + "left join suppliers s on p.product_supplier_id = s.supplier_id "
                            + "order by p.product_id "
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        product = new Product(
                                  reader["product_name"].ToString(),
                                  Convert.ToDouble(reader["product_price"]),
                                  Convert.ToInt32(reader["product_quantity"]),
                                  reader["product_description"].ToString(),
                                  DatabaseClientCast.DBToValue<int>(reader["product_category_id"]),
                                  DatabaseClientCast.DBToValue<int>(reader["product_supplier_id"])
                                  );

                        product.SetProductID((int)reader["product_id"]);

                        list.Add(product);
                    }
                }
            }

            return list;
        }

        public void GetSupplier(ref ObservableCollection<SupplierData> list)
        {
            list = new ObservableCollection<SupplierData>();

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select s.supplier_id, s.supplier_name "
                            + "from suppliers s "
                            + "order by s.supplier_id"
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
                            new SupplierData(
                                (int?)reader["supplier_id"],
                                (string)reader["supplier_name"]
                                ));
                    }
                }
            }
        }

        public void GetCategories(ref ObservableCollection<CategoryData> list)
        {
            list = new ObservableCollection<CategoryData>();

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select c.category_id, c.category_name "
                            + "from categories c "
                            + "order by c.category_id"
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
                            new CategoryData(
                                (int?)reader["category_id"],
                                (string)reader["category_name"]
                                ));
                    }
                }
            }
        }

        #region Speicher Routine
        public void SaveProductList(ref ObservableCollection<Product> products)
        {
            // Produkt aus der Datenbank löschen implementieren

            foreach (Product p in products)
            {
                if (p.ProductID > 0)
                {
                    this.UpdateProduct(p);
                }
                else
                {
                    this.InsertProduct(p);
                }
            }
        }

        private void DeleteProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            sql = "delete from Products "
                + "where ProductID = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ProductID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            sql = "update products set product_name = @productName, product_price = @price, product_quantity = @quantity, product_description = @description, "
                + "                     product_category_id = @categoryID, product_supplier_id = @supplierID "
                + "where product_id = @id";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ProductID;
            cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
            cmd.Parameters.Add("@price", SqlDbType.Money).Value = product.Price;
            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
            cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description.StringToDb();
            cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();
            cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID.ValueToDb<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            sql = "insert into products(product_name, product_price, product_quantity, product_description, product_category_id, product_supplier_id) "
                + "             values (@productName, @price, @quantity, @description, @categoryID, @supplierID)";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
            cmd.Parameters.Add("@price", SqlDbType.Money).Value = product.Price;
            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
            cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description.StringToDb();
            cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();
            cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID.ValueToDb<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand();
                cmd.CommandText = "select @@IDENTITY";
                cmd.Connection = conn;

                product.SetProductID(Convert.ToInt32(cmd.ExecuteScalar()));
            }
        }
        #endregion Speicher Routine
    }
}