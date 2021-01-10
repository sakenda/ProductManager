using ProductManager.Model.User;
using ProductManager.Model.User.Metadata;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ProductManager.ViewModel.Database
{
    public class DatabaseUserQueries : DatabaseProperties
    {
        public void GetUsers(ref ObservableCollection<UserModel> list)
        {
            list = new ObservableCollection<UserModel>();

            string sql = "SELECT                                                                                            "
                       + "	    us.user_id, us.user_firstname, us.user_lastname, us.user_email,                             "
                       + "	    ad.adress_street, ad.adress_number, ad.adress_city, ad.adress_zip, ad.adress_country,       "
                       + "	    pa.payment_cardtype, pa.payment_bic, pa.payment_bankname                                    "
                       + "FROM                                                                                              "
                       + "	    users us                                                                                    "
                       + "	    	LEFT JOIN adresses ad ON us.user_id = ad.adress_id                                      "
                       + "	    	LEFT JOIN userpayment upa ON us.user_id = upa.fk_user_id                                "
                       + "	    	LEFT JOIN payment pa ON upa.fk_payment_id = pa.payment_id	                            "
                       + "	    	LEFT JOIN usersarchived uar ON us.user_id = uar.fk_user_id                              "
                       + "WHERE                                                                                             "
                       + "	    uar.fk_user_id IS NULL;                                                                     ";

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel(
                            reader["user_firstname"].ToString(),
                            reader["user_lastname"].ToString(),
                            reader["user_email"].ToString(),
                            new AdressModel(
                                reader["adress_street"].ToString(),
                                reader["adress_number"].ToString(),
                                reader["adress_city"].ToString(),
                                reader["adress_zip"].ToString(),
                                reader["adress_country"].ToString()),
                            new PaymentModel(
                                reader["payment_cardtype"].ToString(),
                                reader["payment_bic"].ToString(),
                                reader["payment_bankname"].ToString())
                            );

                        user.SetID(DatabaseClientCast.DBToValue<int>(reader["user_id"]));
                        list.Add(user);
                    }
                }
            }
        }
    }
}