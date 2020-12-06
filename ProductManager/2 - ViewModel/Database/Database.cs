using ProductManager.Model.Product;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Data;

namespace ProductManager.ViewModel.DatabaseData
{
    public class Database : DatabaseProperties
    {
        public ObservableCollection<ProductFullDetail> CurrentProducts { get; private set; }
        public ObservableCollection<ProductFullDetail> DeletedProducts { get; private set; }

        #region Singleton

        private static Database _instance = null;

        public static Database Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Database();
                    _instance.GetFullDetailProducts();
                }
                return _instance;
            }
        }

        private Database()
        {
            this.CurrentProducts = new ObservableCollection<ProductFullDetail>();
            this.DeletedProducts = new ObservableCollection<ProductFullDetail>();
        }

        #endregion Singleton

        public void GetFilteredFullDetailProducts(int? categoryId = null, int? supplierId = null)
        {
            (bool needUpdate, int amount) checkIsDirty = CheckIsDirty();

            if (!checkIsDirty.needUpdate)
            {
                ProductFullDetail product;
                StringBuilder builder = new StringBuilder();

                bool isCategory = categoryId == null ? false : true;
                bool isSupplier = supplierId == null ? false : true;

                builder.Append("select p.product_id, p.product_name, p.product_price, p.product_quantity, p.product_description, p.product_category_id, p.product_supplier_id, c.category_name, s.supplier_name ");
                builder.Append("from products p ");
                builder.Append("left join categories c on c.category_id = p.product_category_id ");
                builder.Append("left join suppliers s on s.supplier_id = p.product_supplier_id ");

                if (isCategory)
                {
                    builder.Append($"where p.product_category_id = {categoryId} ");

                    if (isSupplier)
                    {
                        builder.Append($"and p.product_supplier_id = { supplierId} ");
                    }
                }
                else
                {
                    builder.Append($"where p.product_supplier_id = { supplierId} ");
                }

                builder.Append("order by p.product_id ");

                ClearProductLists();

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = builder.ToString()
                };

                using (SqlConnection conn = new SqlConnection(DBCONNECTION))
                {
                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product = new ProductFullDetail(
                                      reader["product_name"].ToString(),
                                      Convert.ToDouble(reader["product_price"]),
                                      Convert.ToInt32(reader["product_quantity"]),
                                      reader["product_description"].ToString(),
                                      DatabaseClientCast.DBToValue<int>(reader["product_category_id"]),
                                      DatabaseClientCast.DBToValue<int>(reader["product_supplier_id"])
                                      );

                            product.SetProductID((int)reader["product_id"]);

                            CurrentProducts.Add(product);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Datensätze nicht gespeichert");
            }
        }

        public void GetFullDetailProducts()
        {
            (bool needUpdate, int amount) checkIsDirty = CheckIsDirty();

            if (!checkIsDirty.needUpdate)
            {
                ProductFullDetail product;

                ClearProductLists();

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
                            product = new ProductFullDetail(
                                      reader["product_name"].ToString(),
                                      Convert.ToDouble(reader["product_price"]),
                                      Convert.ToInt32(reader["product_quantity"]),
                                      reader["product_description"].ToString(),
                                      DatabaseClientCast.DBToValue<int>(reader["product_category_id"]),
                                      DatabaseClientCast.DBToValue<int>(reader["product_supplier_id"])
                                      );

                            product.SetProductID((int)reader["product_id"]);

                            CurrentProducts.Add(product);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Datensätze nicht gespeichert");
            }
        }

        public void ClearProductLists()
        {
            this.CurrentProducts.Clear();
            this.DeletedProducts.Clear();
        }

        public void SaveProductList()
        {
            foreach (ProductFullDetail p in this.DeletedProducts)
            {
                this.DeleteProduct(p);
            }

            foreach (ProductFullDetail p in this.CurrentProducts)
            {
                if (!p.isDirty)
                {
                    continue;
                }

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

        private (bool needUpdate, int amountNeedUpdate) CheckIsDirty()
        {
            bool needUpdate = false;
            int amountNeedUpdate = 0;

            foreach (var itemCur in CurrentProducts)
            {
                if (DeletedProducts.Count != 0)
                {
                    needUpdate = true;
                    amountNeedUpdate += DeletedProducts.Count;
                }

                if (itemCur.isDirty == false)
                {
                    continue;
                }
                else
                {
                    needUpdate = true;
                    amountNeedUpdate++;
                }
            }

            return (needUpdate, amountNeedUpdate);
        }

        private void DeleteProduct(ProductFullDetail product)
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

                product.ResetIsDirty();
            }
        }

        private void UpdateProduct(ProductFullDetail product)
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

                product.ResetIsDirty();
            }
        }

        private void InsertProduct(ProductFullDetail product)
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
                product.ResetIsDirty();
            }
        }
    }
}