using System;
using Microsoft.Data.SqlClient;

namespace FinanceManagement.Database
{
    public static class DbConnectionHelper
    {
        private static readonly string connectionString =
            "Server=LAPTOP-HMGHO6NI;Database=FinanceManagement;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();  
            return conn;
        }
    }
}
