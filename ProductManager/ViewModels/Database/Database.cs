using ProductManager.Models.Product;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProductManager.ViewModels.DatabaseData
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

                builder.Append("select p.ProductID, p.ProductName, p.Price, p.Quantity, p.Description, p.CategoryID, p.SupplierID, c.CategoryName, s.SupplierName ");
                builder.Append("from Products p ");
                builder.Append("left join Categories c on c.CategoryID = p.CategoryID ");
                builder.Append("left join Suppliers s on s.SupplierID = p.SupplierID ");

                if (isCategory)
                {
                    builder.Append($"where p.CategoryID = {categoryId} ");

                    if (isSupplier)
                    {
                        builder.Append($"and p.SupplierID = { supplierId} ");
                    }
                }
                else
                {
                    builder.Append($"where p.SupplierID = { supplierId} ");
                }

                builder.Append("order by p.ProductID ");

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
                                      reader[nameof(product.ProductName)].ToString(),
                                      Convert.ToDouble(reader[nameof(product.Price)]),
                                      Convert.ToInt32(reader[nameof(product.Quantity)]),
                                      reader[nameof(product.Description)].ToString(),
                                      DatabaseClientCast.DBToValue<int>(reader["CategoryID"]),
                                      DatabaseClientCast.DBToValue<int>(reader["SupplierID"])
                                      );

                            product.SetProductID((int)reader[nameof(product.ProductID)]);

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
                    CommandText = "select p.ProductID, p.ProductName, p.Price, p.Quantity, p.Description, "
                                + "p.CategoryID, c.CategoryName, s.SupplierID, s.SupplierName "
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
                            product = new ProductFullDetail(
                                      reader[nameof(product.ProductName)].ToString(),
                                      Convert.ToDouble(reader[nameof(product.Price)]),
                                      Convert.ToInt32(reader[nameof(product.Quantity)]),
                                      reader[nameof(product.Description)].ToString(),
                                      DatabaseClientCast.DBToValue<int>(reader["CategoryID"]),
                                      DatabaseClientCast.DBToValue<int>(reader["SupplierID"])
                                      );

                            product.SetProductID((int)reader[nameof(product.ProductID)]);

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

            sql = "update Products set ProductName = @productName, Price = @price, Quantity = @quantity, Description = @description, "
                + "                     CategoryID = @categoryID, SupplierID = @supplierID "
                + "where ProductID = @id";

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

            sql = "insert into Products(ProductName, Price, Quantity, Description, CategoryID, SupplierID) "
                + "         values (@productName, @price, @quantity, @description, @categoryID, @supplierID)";

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