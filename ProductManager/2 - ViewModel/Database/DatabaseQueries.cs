using ProductManager.Model.Product;
using ProductManager.Model.Product.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace ProductManager.ViewModel.Database
{
    public class DatabaseQueries : DatabaseProperties
    {
        #region MetaData
        public void GetSupplier(ref ObservableCollection<SupplierData> list)
        {
            list = new ObservableCollection<SupplierData>();

            SqlCommand cmd = new SqlCommand("")
            {
                CommandText = "SELECT "
                            + "supplier_id, "
                            + "supplier_name, "
                            + "supplier_street, "
                            + "supplier_nr, "
                            + "supplier_city, "
                            + "supplier_zip "
                            + "FROM "
                            + "suppliers; "
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
                                reader["supplier_city"].ToString(),
                                null, null, null
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

            sql = "INSERT INTO suppliers (supplier_name, supplier_street, supplier_nr, supplier_city, supplier_zip)     "
                + " 	VALUES (@name, @street, @nr, @city, @zip);                                                      ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@name", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Name);
            cmd.Parameters.Add("@street", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Street);
            cmd.Parameters.Add("@nr", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Nr);
            cmd.Parameters.Add("@city", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.City);
            cmd.Parameters.Add("@zip", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Zip);

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
                CommandText = "SELECT "
                            + "category_id, "
                            + "category_name, "
                            + "category_description "
                            + "FROM categories; "
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

            sql = "INSERT INTO categories (category_name, category_description)     "
                + " 	VALUES (@name, @description);                               ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@name", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Name);
            cmd.Parameters.Add("@description", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Description);

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
                CommandText = "SELECT                                                                                           "
                            + "     p.product_id, p.product_name, p.product_quantity, p.product_description,                    "
                            + "     pr.price_base, pr.price_shipping, pr.price_profit,                                          "
                            + "     cat.category_id, cat.category_name, cat.category_description,                               "
                            + "     sup.supplier_id, sup.supplier_name, sup.supplier_street, sup.supplier_nr,                   "
                            + "     sup.supplier_city, sup.supplier_zip,                                                        "
                            + "     img.image_name                                                                              "
                            + "FROM                                                                                             "
                            + "     products p                                                                                  "
                            + "         LEFT JOIN prices pr             ON p.product_id         = pr.fk_product_id              "
                            + "         LEFT JOIN productcategory pcat  ON p.product_id         = pcat.fk_product_id            "
                            + "         LEFT JOIN categories cat        ON pcat.fk_category_id  = cat.category_id               "
                            + "         LEFT JOIN productsupplier psup  ON p.product_id         = psup.fk_product_id            "
                            + "         LEFT JOIN suppliers sup         ON psup.fk_supplier_id  = sup.supplier_id               "
                            + "         LEFT JOIN productimage pimg     ON pimg.fk_product_id   = p.product_id                  "
                            + "         LEFT JOIN images img            ON pimg.fk_image_id     = img.image_id                  "
                            + "         LEFT JOIN productsarchived pa   ON p.product_id         = pa.fk_product_id              "
                            + "WHERE                                                                                            "
                            + "     pa.fk_product_id IS NULL;                                                                   "
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
                            Convert.ToDecimal(DatabaseClientCast.DBToValue<decimal>(reader["price_profit"]))
                            );

                        image = new ImageModel(
                            reader["image_name"].ToString()
                            );

                        product = new ProductModel(
                                  reader["product_name"].ToString(),
                                  price,
                                  Convert.ToInt32(reader["product_quantity"]),
                                  reader["product_description"].ToString(),
                                  DatabaseClientCast.DBToValue<int>(reader["category_id"]),
                                  DatabaseClientCast.DBToValue<int>(reader["supplier_id"]),
                                  image
                                  );

                        product.SetID((int)reader["product_id"]);

                        list.Add(product);
                    }
                }
            }

            return list;
        }

        public void SaveProductList(ref List<ProductModel> changedProducts, ref List<ProductModel> deletedProducts)
        {
            if (deletedProducts.Count > 0)
            {
                foreach (ProductModel product in deletedProducts)
                {
                    this.DeleteProduct(product);
                }
            }

            foreach (ProductModel p in changedProducts)
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
            sql = "INSERT INTO productsarchived VALUES (@productID); ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Delete Produkt
        }

        private void UpdateProduct(ProductModel product)
        {
            string sql;
            SqlCommand cmd;

            #region Update Produkt
            sql = "UPDATE products                              "
                + "SET product_name = @name,                    "
                + "    product_quantity = @quantity,            "
                + "	   product_description = @description       "
                + "WHERE product_id = @productID;               ";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
            cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description.StringToDb();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Update Produkt

            #region Update Price
            sql = "UPDATE prices                            "
                + "SET price_base = @base,                  "
                + "    price_shipping = @shipping,          "
                + "    price_profit = @profit               "
                + "WHERE fk_product_id = @productID;        ";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@base", SqlDbType.Money).Value = product.Price.BasePrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@shipping", SqlDbType.Money).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@profit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Update Price

            #region Update Category
            sql = "UPDATE productcategory                   "
                + "SET fk_category_id = @categoryID         "
                + "WHERE fk_product_id = @productID;        ";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Update Category

            #region Update Supplier
            sql = "UPDATE productsupplier                 "
                + "SET fk_supplier_id = @supplierID       "
                + "WHERE fk_product_id = @productID;      ";

            cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Update Supplier

            #region Update Image
            // Dateiname in Datenbank speichern
            sql = "UPDATE images                    "
                + "SET image_name = @imageName      "
                + "WHERE image_id = @imageID;       ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID.ValueToDb<int>();
            cmd.Parameters.Add("@imageName", SqlDbType.Text).Value = product.Image.FileName.StringToDb();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }

            // Bild und Produkt verknüpfen
            sql = "UPDATE productimage                      "
                + "SET fk_image_id = @imageID               "
                + "WHERE fk_product_id = @productID;        ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID;
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

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
            sql = "INSERT INTO products (product_name, product_quantity, product_description)       "
                + "     VALUES (@name, @quantity, @description);                                    ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@name", SqlDbType.Text).Value = product.ProductName.StringToDb();
            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = DatabaseClientCast.ValueToDb<int>(product.Quantity);
            cmd.Parameters.Add("@description", SqlDbType.Text).Value = product.Description.StringToDb();

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

            #region Insert Price
            sql = "INSERT INTO prices (price_base, price_shipping, price_profit, fk_product_id)       "
                + "     VALUES (@base, @shipping, @profit, @productID);                                 ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@base", SqlDbType.Decimal).Value = product.Price.BasePrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@shipping", SqlDbType.Decimal).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
            cmd.Parameters.Add("@profit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Insert Price

            #region Insert Category
            sql = "INSERT INTO productcategory (fk_category_id, fk_product_id)      "
                + " 	VALUES (@categoryID, @productID);                           ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID.ValueToDb<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Insert Category

            #region Insert Supplier
            sql = "INSERT INTO productsupplier (fk_supplier_id, fk_product_id)      "
                + " 	VALUES (@supplierID, @productID);                            ";

            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID;
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            #endregion Insert Supplier

            #region Insert Image
            sql = "INSERT INTO images (image_name) VALUES (@name); ";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@name", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(product.Image.FileName);

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand();
                cmd.CommandText = "select @@IDENTITY";
                cmd.Connection = conn;

                id = Convert.ToInt32(cmd.ExecuteScalar());
                product.Image.SetID(id);
            }

            sql = "INSERT INTO productimage (fk_image_id, fk_product_id) VALUES (@imageID, @productID); ";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID;
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

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