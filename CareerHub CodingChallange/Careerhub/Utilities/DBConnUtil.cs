using Microsoft.Data.SqlClient;

namespace CareerHub.Utilities
{
    public static class DBUtil
    {
        private static readonly string connectionString =
            @"Server=LAPTOP-HMGHO6NI;Database=Careerhub;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetDBConn()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}