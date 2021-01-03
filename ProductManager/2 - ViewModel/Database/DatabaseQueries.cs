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
            #region Insert Supplier [dbo.suppliers]
            string sql;
            SqlCommand cmd;

            sql = "INSERT INTO suppliers (supplier_name, supplier_street, supplier_nr, supplier_city, supplier_zip)     "
                + " 	VALUES (@name, @street, @nr, @city, @zip);                                                      ";

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@name", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Name);
                cmd.Parameters.Add("@street", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Street);
                cmd.Parameters.Add("@nr", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Nr);
                cmd.Parameters.Add("@city", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.City);
                cmd.Parameters.Add("@zip", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Zip);

                ExecuteQueryTransaction(cmd);
            }
            #endregion Insert Supplier [dbo.suppliers]
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
                                reader["category_name"].ToString(),
                                reader["category_description"].ToString()
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
            #region Insert Category [dbo.categories]
            string sql;
            SqlCommand cmd;

            sql = "INSERT INTO categories (category_name, category_description)     "
                + " 	VALUES (@name, @description);                               ";

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@name", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Name);
                cmd.Parameters.Add("@description", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(data.Description);

                ExecuteQueryTransaction(cmd);
            }
            #endregion Insert Category [dbo.categories]
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
                            + "     img.image_id, img.image_name                                                                "
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
                                  Convert.ToInt32(reader["product_quantity"]),
                                  reader["product_description"].ToString(),
                                  price,
                                  image,
                                  DatabaseClientCast.DBToValue<int>(reader["category_id"]),
                                  DatabaseClientCast.DBToValue<int>(reader["supplier_id"])
                                  );

                        product.SetID((int)reader["product_id"]);
                        product.Image.SetID(DatabaseClientCast.DBToValue<int>(reader["image_id"]));

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

        private void ExecuteQueryTransaction(SqlCommand cmd)
        {
            SqlTransaction transaction;
            transaction = cmd.Connection.BeginTransaction();
            cmd.Transaction = transaction;

            try
            {
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
        }

        private void DeleteProduct(ProductModel product)
        {
            string sql;
            SqlCommand cmd;

            sql = "INSERT INTO productsarchived (fk_product_id) VALUES (@productID); ";

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

                ExecuteQueryTransaction(cmd);
            }
        }

        private void UpdateProduct(ProductModel product)
        {
            string sql;
            SqlCommand cmd;
            SqlTransaction transaction;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);

                try
                {
                    #region Update Produkt
                    sql = "UPDATE products                              "
                        + "SET product_name = @name,                    "
                        + "    product_quantity = @quantity,            "
                        + "	   product_description = @description       "
                        + "WHERE product_id = @productID;               ";

                    cmd = new SqlCommand(sql, conn, transaction);
                    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description.StringToDb();

                    cmd.ExecuteNonQuery();
                    #endregion Update Produkt

                    #region Update Price
                    sql = "UPDATE prices                            "
                        + "SET price_base = @base,                  "
                        + "    price_shipping = @shipping,          "
                        + "    price_profit = @profit               "
                        + "WHERE fk_product_id = @productID;        ";

                    cmd = new SqlCommand(sql, conn, transaction);
                    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
                    cmd.Parameters.Add("@base", SqlDbType.Money).Value = product.Price.BasePrice.ValueToDb<decimal>();
                    cmd.Parameters.Add("@shipping", SqlDbType.Money).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
                    cmd.Parameters.Add("@profit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();
                    #endregion Update Price

                    #region Update Category [dbo.productscategory]
                    if (product.CategoryID != null)
                    {
                        sql = "UPDATE productcategory                                                   "
                            + "SET fk_category_id = @categoryID                                         "
                            + "WHERE fk_product_id = @productID                                         "
                            + "IF @@ROWCOUNT = 0                                                        "
                            + "     INSERT INTO productcategory VALUES(@productID, @categoryID);        ";

                        cmd = new SqlCommand(sql, conn, transaction);
                        cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID.ValueToDb<int>();
                        cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();

                        cmd.ExecuteNonQuery();
                    }
                    #endregion Update Category [dbo.productscategory]

                    #region Update Supplier [dbo.productssupplier]
                    if (product.SupplierID != null)
                    {
                        sql = "UPDATE productsupplier                                                   "
                            + "SET fk_supplier_id = @supplierID                                         "
                            + "WHERE fk_product_id = @productID                                         "
                            + "IF @@ROWCOUNT = 0                                                        "
                            + "     INSERT INTO productcategory VALUES(@productID, @supplierID);        ";

                        cmd = new SqlCommand(sql, conn, transaction);
                        cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID.ValueToDb<int>();
                        cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID.ValueToDb<int>();

                        cmd.ExecuteNonQuery();
                    }
                    #endregion Update Supplier [dbo.productssupplier]

                    if (product.Image.ID != null)
                    {
                        #region [dbo.images]
                        sql = "UPDATE images                                    "
                            + "SET image_name = @imageName                      "
                            + "WHERE image_id = @imageID;                       "
                            + "IF @@ROWCOUNT = 0                                "
                            + "     INSERT INTO images VALUES (@imageName);     ";

                        cmd = new SqlCommand(sql, conn, transaction);
                        cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID.ValueToDb<int>();
                        cmd.Parameters.Add("@imageName", SqlDbType.Text).Value = product.Image.FileName.StringToDb();

                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("select @@IDENTITY", conn, transaction);
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        if (product.Image.ID != id)
                        {
                            product.Image.SetID(id);
                        }
                        #endregion [dbo.images]

                        #region [dbo.productimage]
                        sql = "UPDATE productimage                      "
                            + "SET fk_image_id = @imageID               "
                            + "WHERE fk_product_id = @productID;        "
                            + "IF @@ROWCOUNT = 0                        "
                            + "     INSERT INTO productimage VALUES (@productID, @imageID); ";

                        cmd = new SqlCommand(sql, conn, transaction);
                        cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID.ValueToDb<int>();
                        cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

                        cmd.ExecuteNonQuery();
                        #endregion [dbo.productimage]
                    }
                    else
                    {
                        sql = "UPDATE productimage                      "
                            + "SET fk_image_id = @imageID               "
                            + "WHERE fk_product_id = @productID;        "
                            + "IF @@ROWCOUNT = 0                        "
                            + "     INSERT INTO productimage VALUES (@productID, @imageID); ";

                        cmd = new SqlCommand(sql, conn, transaction);
                        cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID.ValueToDb<int>();
                        cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        throw ex2;
                    }
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            #region ============= Backup ================================================================================================
            //string sql;
            //SqlCommand cmd;

            //#region Update Produkt
            //sql = "UPDATE products                              "
            //    + "SET product_name = @name,                    "
            //    + "    product_quantity = @quantity,            "
            //    + "	   product_description = @description       "
            //    + "WHERE product_id = @productID;               ";

            //using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            //{
            //    conn.Open();
            //    cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            //    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.ProductName.StringToDb();
            //    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
            //    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description.StringToDb();

            //    ExecuteQueryTransaction(cmd);
            //}
            //#endregion Update Produkt

            //#region Update Price
            //sql = "UPDATE prices                            "
            //    + "SET price_base = @base,                  "
            //    + "    price_shipping = @shipping,          "
            //    + "    price_profit = @profit               "
            //    + "WHERE fk_product_id = @productID;        ";

            //using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            //{
            //    conn.Open();
            //    cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            //    cmd.Parameters.Add("@base", SqlDbType.Money).Value = product.Price.BasePrice.ValueToDb<decimal>();
            //    cmd.Parameters.Add("@shipping", SqlDbType.Money).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
            //    cmd.Parameters.Add("@profit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();

            //    ExecuteQueryTransaction(cmd);
            //}
            //#endregion Update Price

            //#region Update Category [dbo.productscategory]
            //sql = "UPDATE productcategory                                                   "
            //    + "SET fk_category_id = @categoryID                                         "
            //    + "WHERE fk_product_id = @productID                                         "
            //    + "IF @@ROWCOUNT = 0                                                        "
            //    + "     INSERT INTO productcategory VALUES(@productID, @categoryID);        ";

            //using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            //{
            //    conn.Open();
            //    cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            //    cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();

            //    ExecuteQueryTransaction(cmd);
            //}
            //#endregion Update Category [dbo.productscategory]

            //#region Update Supplier [dbo.productssupplier]
            //sql = "UPDATE productsupplier                                                   "
            //    + "SET fk_supplier_id = @supplierID                                         "
            //    + "WHERE fk_product_id = @productID                                         "
            //    + "IF @@ROWCOUNT = 0                                                        "
            //    + "     INSERT INTO productcategory VALUES(@productID, @supplierID);        ";

            //using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            //{
            //    conn.Open();
            //    cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            //    cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID.ValueToDb<int>();

            //    ExecuteQueryTransaction(cmd);
            //}
            //#endregion Update Supplier [dbo.productssupplier]

            //#region Update Image
            //using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            //{
            //    int? newImageID;

            //    #region [dbo.images]
            //    sql = "UPDATE images                                    "
            //        + "SET image_name = @imageName                      "
            //        + "WHERE image_id = @imageID;                       "
            //        + "IF @@ROWCOUNT = 0                                "
            //        + "     INSERT INTO images VALUES (@imageName);     ";

            //    conn.Open();
            //    cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID.ValueToDb<int>();
            //    cmd.Parameters.Add("@imageName", SqlDbType.Text).Value = product.Image.FileName.StringToDb();

            //    ExecuteQueryTransaction(cmd);

            //    cmd = new SqlCommand("select @@IDENTITY", conn);
            //    newImageID = Convert.ToInt32(DatabaseClientCast.DBToValue<int>(cmd.ExecuteScalar()));
            //    #endregion [dbo.images]

            //    #region [dbo.productimage]
            //    sql = "UPDATE productimage                      "
            //        + "SET fk_image_id = @imageID               "
            //        + "WHERE fk_product_id = @productID;        "
            //        + "IF @@ROWCOUNT = 0                        "
            //        + "     INSERT INTO productimage VALUES (@productID, @newImageID); ";

            //    cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID;
            //    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;
            //    cmd.Parameters.Add("@newImageID", SqlDbType.Int).Value = newImageID;

            //    ExecuteQueryTransaction(cmd);
            //    #endregion [dbo.productimage]
            //}
            //#endregion Update Image
            #endregion ============= Backup ================================================================================================
        }

        private void InsertProduct(ProductModel product)
        {
            string sql;
            int id;
            SqlConnection conn;
            SqlCommand cmd;

            #region Insert Produkt
            sql = "INSERT INTO products (product_name, product_quantity, product_description)       "
                + "     VALUES (@name, @quantity, @description);                                    ";

            using (conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@name", SqlDbType.Text).Value = product.ProductName.StringToDb();
                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = DatabaseClientCast.ValueToDb<int>(product.Quantity);
                cmd.Parameters.Add("@description", SqlDbType.Text).Value = product.Description.StringToDb();

                ExecuteQueryTransaction(cmd);

                cmd = new SqlCommand("select @@IDENTITY", conn);
                id = Convert.ToInt32(cmd.ExecuteScalar());

                product.SetID(id);
            }
            #endregion Insert Produkt

            #region Insert Price
            sql = "INSERT INTO prices (price_base, price_shipping, price_profit, fk_product_id)         "
                + "     VALUES (@base, @shipping, @profit, @productID);                                 ";

            using (conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@base", SqlDbType.Decimal).Value = product.Price.BasePrice.ValueToDb<decimal>();
                cmd.Parameters.Add("@shipping", SqlDbType.Decimal).Value = product.Price.ShippingPrice.ValueToDb<decimal>();
                cmd.Parameters.Add("@profit", SqlDbType.Decimal).Value = product.Price.Profit.ValueToDb<decimal>();
                cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

                ExecuteQueryTransaction(cmd);
            }
            #endregion Insert Price

            #region Insert Category [dbo.productcategory]
            if (product.CategoryID != null)
            {
                sql = "INSERT INTO productcategory (fk_category_id, fk_product_id)      "
                    + " 	VALUES (@categoryID, @productID);                           ";

                using (conn = new SqlConnection(DBCONNECTION))
                {
                    conn.Open();
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = product.CategoryID.ValueToDb<int>();
                    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID.ValueToDb<int>();

                    ExecuteQueryTransaction(cmd);
                }
            }
            #endregion Insert Category [dbo.productcategory]

            #region Insert Supplier [dbo.productsupplier]
            if (product.SupplierID != null)
            {
                sql = "INSERT INTO productsupplier (fk_supplier_id, fk_product_id)      "
                    + " 	VALUES (@supplierID, @productID);                            ";

                using (conn = new SqlConnection(DBCONNECTION))
                {
                    conn.Open();
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@supplierID", SqlDbType.Int).Value = product.SupplierID;
                    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

                    ExecuteQueryTransaction(cmd);
                }
            }
            #endregion Insert Supplier [dbo.productsupplier]

            #region Insert Image
            if (product.Image.ID != null)
            {
                using (conn = new SqlConnection(DBCONNECTION))
                {
                    #region [dbo.images]
                    sql = "INSERT INTO images (image_name) VALUES (@name); ";

                    conn.Open();
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@name", SqlDbType.Text).Value = DatabaseClientCast.StringToDb(product.Image.FileName);

                    ExecuteQueryTransaction(cmd);

                    cmd = new SqlCommand("select @@IDENTITY", conn);
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    product.Image.SetID(id);
                    #endregion [dbo.images]

                    #region [dbo.productimage]
                    sql = "INSERT INTO productimage (fk_image_id, fk_product_id) VALUES (@imageID, @productID); ";

                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@imageID", SqlDbType.Int).Value = product.Image.ID;
                    cmd.Parameters.Add("@productID", SqlDbType.Int).Value = product.ID;

                    ExecuteQueryTransaction(cmd);
                    #endregion [dbo.productimage]
                }
            }
            #endregion Insert Image
        }
        #endregion Products
    }
}