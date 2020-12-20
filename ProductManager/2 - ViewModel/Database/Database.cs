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
        #region MetaData
        public void GetSupplier(ref ObservableCollection<SupplierData> list)
        {
            list = new ObservableCollection<SupplierData>();

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select s.supplier_id, s.supplier_name, s.supplier_address "
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
                                DatabaseClientCast.DBToValue<int>(reader["supplier_id"]),
                                reader["supplier_name"].ToString(),
                                reader["supplier_address"].ToString()
                                ));
                    }
                }
            }
        }
        public void DeleteSupplier(SupplierData data)
        {
            string sql;
            SqlCommand cmd;

            sql = "delete from suppliers "
                + "where supplier_id = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }
        public void InsertSupplier(SupplierData data)
        {
            string sql;
            SqlCommand cmd;

            sql = "INSERT into suppliers(supplier_name, supplier_address) "
                + "               values(@NAME, @ADDRESS)";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@NAME", SqlDbType.Text).Value = data.Name;
            cmd.Parameters.Add("@ADDRESS", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Address);

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        public void GetCategories(ref ObservableCollection<CategoryData> list)
        {
            list = new ObservableCollection<CategoryData>();

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select c.category_id, c.category_name, c.category_description "
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
                                DatabaseClientCast.DBToValue<int>(reader["category_id"]),
                                (string)reader["category_name"].ToString(),
                                (string)reader["category_description"].ToString()
                                ));
                    }
                }
            }
        }
        public void DeleteCategory(CategoryData data)
        {
            string sql;
            SqlCommand cmd;

            sql = "delete from categories "
                + "where category_id = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }
        public void InsertCategory(CategoryData data)
        {
            string sql;
            SqlCommand cmd;

            sql = "INSERT into categories(category_name, category_description) "
                + "                values(@NAME, @DESCRIPTION)";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@NAME", SqlDbType.Text).Value = data.Name;
            cmd.Parameters.Add("@DESCRIPTION", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Description);

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        #endregion MetaData

        #region Products
        public ObservableCollection<ProductModel> GetProducts()
        {
            ObservableCollection<ProductModel> list = new ObservableCollection<ProductModel>();
            ProductModel product;
            PriceModel price;
            ImageModel image;

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "select product.product_id, "
                            + "product.product_name, "
                            + "product.product_quantity, "
                            + "product.product_description, "
                            + "product.product_category_id, "
                            + "product.product_supplier_id, "
                            + "category.category_name, "
                            + "supplier.supplier_name, "
                            + "price.price_base, "
                            + "price.price_shipping, "
                            + "price.price_profit, "
                            + "p_image.image_path "
                            + "from Products product "
                            + "left join categories category on product.product_category_id = category.category_id "
                            + "left join suppliers supplier on product.product_supplier_id = supplier.supplier_id "
                            + "left join prices price on product.product_id = price.price_id "
                            + "left join product_images p_image on product.product_id = p_image.image_id"
            };

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        price = new PriceModel(
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_base"])),
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_shipping"])),
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_base"]))
                            );

                        image = new ImageModel(
                            reader["image_path"].ToString()
                            );

                        product = new ProductModel(
                                  reader["product_name"].ToString(),
                                  price,
                                  Convert.ToInt32(reader["product_quantity"]),
                                  reader["product_description"].ToString(),
                                  DatabaseClientCast.DBToValue<int>(reader["product_category_id"]),
                                  DatabaseClientCast.DBToValue<int>(reader["product_supplier_id"]),
                                  image
                                  );

                        product.SetID((int)reader["product_id"]);

                        list.Add(product);
                    }
                }
            }

            return list;
        }

        public void SaveProductList(ref List<ProductModel> products, ref List<ProductModel> deletedProducts)
        {
            if (deletedProducts.Count > 0)
            {
                foreach (ProductModel product in deletedProducts)
                {
                    this.DeleteProduct(product);
                }
            }

            foreach (ProductModel p in products)
            {
                if (p.ID > 0)
                {
                    this.UpdateProduct(p);
                }
                else
                {
                    this.InsertProduct(p);
                }
            }
        }

        private void DeleteProduct(ProductModel product)
        {
            string sql;
            SqlCommand cmd;

            #region Delete Produkt
            sql = "delete from products "
                + "where product_id = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ID;

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
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Delete Preis

            #region Delete Image
            sql = "delete from product_images "
                + "where image_id = @id";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Delete Image
        }

        private void UpdateProduct(ProductModel product)
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

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ID;
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
                + "price_profit = @priceProfit "
                + "WHERE price_id = @productID";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
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

            #region Update Image
            sql = "UPDATE product_images set image_path = @imagePath "
                + "WHERE image_id = @productID "
                + "IF @@ROWCOUNT = 0 "
                + "INSERT into product_images(image_id, image_path) "
                + "                    values(@productID, @imagePath) ";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@imagePath", SqlDbType.Text).Value = product.Image.Path.StringToDb();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Update Image
        }

        private void InsertProduct(ProductModel product)
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
                product.SetID(id);
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
            }
            #endregion Insert Preis

            #region Insert Image
            sql = "INSERT into product_images(image_id, image_path) "
                + "                    values(@productID, @imagePath)";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@imagePath", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(product.Image.Path);

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Insert Image
        }
        #endregion Products
    }
}