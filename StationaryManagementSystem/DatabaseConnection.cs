using System.Configuration;
using System.Data.SqlClient;

namespace StockManagementSystem
{
    internal class DatabaseConnection
    {
        public static SqlConnection Connection;


        public static string Constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            Connection = new SqlConnection(Constr);
            Connection.Open();
            return Connection;
        }
    }
}