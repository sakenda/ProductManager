using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using System;
using System.Collections.Generic;
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
            Price price;

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select p.product_id, "
                            + "p.product_name, "
                            + "p.product_quantity, "
                            + "p.product_description, "
                            + "p.product_category_id, "
                            + "p.product_supplier_id, "
                            + "c.category_name, "
                            + "s.supplier_name, "
                            + "pri.price_id, "
                            + "pri.price_base, "
                            + "pri.price_shipping, "
                            + "pri.price_profit "
                            + "from Products p "
                            + "left join categories c on p.product_category_id = c.category_id "
                            + "left join suppliers s on p.product_supplier_id = s.supplier_id "
                            + "left join prices pri on p.product_id = pri.price_id "
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
                        price = new Price(
                            DatabaseClientCast.DBToValue<int>(reader["price_id"]),
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_base"])),
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_shipping"])),
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_base"]))
                            );

                        product = new Product(
                                  reader["product_name"].ToString(),
                                  price,
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
        public void SaveProductList(ref List<Product> products, ref List<Product> deletedProducts)
        {
            if (deletedProducts.Count > 0)
            {
                foreach (Product product in deletedProducts)
                {
                    this.DeleteProduct(product);
                }
            }

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

            #region Delete Produkt
            sql = "delete from products "
                + "where product_id = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ProductID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Delete Produkt

            #region Delete Preis
            sql = "delete from prices "
                + "where price_id = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.Price.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Delete Preis
        }

        private void UpdateProduct(Product product)
        {
            string sql;
            SqlCommand cmd;

            #region Update Produkt
            sql = "UPDATE products set "
                + "product_name = @productName, "
                + "product_quantity = @quantity, "
                + "product_description = @description, "
                + "product_category_id = @categoryID, "
                + "product_supplier_id = @supplierID "
                + "WHERE product_id = @id";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ProductID;
            cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
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
            #endregion Update Produkt

            #region Update Price
            sql = "UPDATE prices set "
                + "price_base = @priceBase, "
                + "price_shipping = @priceShipping, "
                + "price_profit = @priceProfit"
                + "WHERE price_id = @priceID";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@priceID", SqlDbType.Int).Value = product.Price.ID;
            cmd.Parameters.Add("@priceBase", SqlDbType.Money).Value = product.Price.BasePrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@priceShipping", SqlDbType.Money).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@priceProfit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Update Price
        }

        private void InsertProduct(Product product)
        {
            string sql;
            int id;
            SqlCommand cmd;

            #region Insert Produkt
            sql = "INSERT into products(product_name, product_quantity, product_description, product_category_id, product_supplier_id) "
                + "              values(@productName, @quantity, @description, @categoryID, @supplierID) ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
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

                id = Convert.ToInt32(cmd.ExecuteScalar());
                product.SetProductID(id);
            }
            #endregion Insert Produkt

            #region Insert Preis
            sql = "INSERT into prices(price_id, price_base, price_shipping, price_profit) "
                + "            values(@priceID, @priceBase, @priceShipping, @priceProfit)";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@priceID", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@priceBase", SqlDbType.Decimal).Value = product.Price.BasePrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@priceShipping", SqlDbType.Decimal).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@priceProfit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                product.Price.SetID(id);
            }
            #endregion Insert Preis
        }
        #endregion Speicher Routine
    }
}