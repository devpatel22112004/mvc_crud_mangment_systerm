using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MVC_CRUD_Demo.Models
{
    /// <summary>
    /// Centralized Database Connection Helper Class
    /// This class provides a single point for all database connections in the project
    /// No need to write connection string code in every controller
    /// </summary>
    public class DatabaseHelper
    {
        // Private static connection string - loaded once from Web.config
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["dbcs"]?.ConnectionString;

        /// <summary>
        /// Get a new SQL Connection instance
        /// Usage: using (SqlConnection con = DatabaseHelper.GetConnection()) { ... }
        /// </summary>
        public static SqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new InvalidOperationException("Connection string 'dbcs' not found in Web.config");
            }

            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Check if connection string is configured properly
        /// Returns true if connection string exists and is not empty
        /// </summary>
        public static bool IsConnectionStringValid()
        {
            return !string.IsNullOrEmpty(ConnectionString);
        }

        /// <summary>
        /// Get the connection string value
        /// Useful for debugging or logging purposes
        /// </summary>
        public static string GetConnectionString()
        {
            return ConnectionString;
        }

        /// <summary>
        /// Test database connection
        /// Returns true if connection can be established, false otherwise
        /// </summary>
        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Execute a SQL query that returns a single value (SELECT COUNT, SUM, etc.)
        /// Usage: int count = (int)DatabaseHelper.ExecuteScalar(query, parameters);
        /// </summary>
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Execute INSERT, UPDATE, DELETE queries
        /// Returns the number of rows affected
        /// Usage: int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
        /// </summary>
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Execute SELECT queries that return multiple rows
        /// Returns SqlDataReader - must be used within using statement
        /// Usage: using (SqlDataReader reader = DatabaseHelper.ExecuteReader(query, parameters)) { ... }
        /// </summary>
        public static SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
        {
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(query, con);

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            con.Open();
            // CommandBehavior.CloseConnection ensures connection is closed when reader is disposed
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
