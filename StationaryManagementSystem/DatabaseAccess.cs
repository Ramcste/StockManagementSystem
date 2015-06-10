using System.Data.SqlClient;
using System.Configuration;
namespace StockManagementSystem
{
    internal class DatabaseAccess
    {
        public SqlConnection Connection;

        public DatabaseAccess()
        {
            Connection = DatabaseConnection.GetConnection();
        }
    }
}