using System.Data.SqlClient;

namespace MVC_CRUD_Demo.Models
{
    /// <summary>
    /// Centralized SQL Query Builder for Common CRUD Operations
    /// Provides reusable SQL queries for standard database operations
    /// 
    /// HOW TO USE:
    /// 
    /// 1. GET ALL USERS:
    ///    string query = SqlQueries.GetAllUsers();
    ///    using (SqlDataReader rd = DatabaseHelper.ExecuteReader(query)) { ... }
    /// 
    /// 2. INSERT USER:
    ///    string query = SqlQueries.InsertUser();
    ///    SqlParameter[] params = SqlQueries.UserParameters(username, password);
    ///    DatabaseHelper.ExecuteNonQuery(query, params);
    /// 
    /// 3. UPDATE USER:
    ///    string query = SqlQueries.UpdateUser();
    ///    SqlParameter[] params = SqlQueries.UserUpdateParameters(id, username, password);
    ///    DatabaseHelper.ExecuteNonQuery(query, params);
    /// 
    /// 4. DELETE USER:
    ///    string query = SqlQueries.DeleteUser();
    ///    SqlParameter[] params = { SqlQueries.IdParameter(id) };
    ///    DatabaseHelper.ExecuteNonQuery(query, params);
    /// 
    /// 5. CHECK LOGIN:
    ///    string query = SqlQueries.ValidateLogin();
    ///    SqlParameter[] params = SqlQueries.LoginParameters(username, password);
    ///    int count = (int)DatabaseHelper.ExecuteScalar(query, params);
    /// 
    /// 6. GENERIC QUERIES (for any table):
    ///    string query = SqlQueries.SelectAll("Products");
    ///    string query = SqlQueries.SelectById("Products");
    ///    string query = SqlQueries.DeleteById("Products");
    /// </summary>
    public static class SqlQueries
    {
        #region User Table Queries

        /// <summary>
        /// Get all users from Users table
        /// Usage: string query = SqlQueries.GetAllUsers();
        /// </summary>
        public static string GetAllUsers()
        {
            return "SELECT * FROM Users";
        }

        /// <summary>
        /// Get user by ID
        /// Usage: string query = SqlQueries.GetUserById(); SqlParameter[] params = { SqlQueries.IdParameter(id) };
        /// </summary>
        public static string GetUserById()
        {
            return "SELECT * FROM Users WHERE Id=@Id";
        }

        /// <summary>
        /// Insert new user
        /// Usage: string query = SqlQueries.InsertUser(); SqlParameter[] params = SqlQueries.UserParameters(username, password);
        /// </summary>
        public static string InsertUser()
        {
            return "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
        }

        /// <summary>
        /// Update existing user
        /// Usage: string query = SqlQueries.UpdateUser(); SqlParameter[] params = SqlQueries.UserUpdateParameters(id, username, password);
        /// </summary>
        public static string UpdateUser()
        {
            return "UPDATE Users SET Username=@Username, Password=@Password WHERE Id=@Id";
        }

        /// <summary>
        /// Delete user by ID
        /// Usage: string query = SqlQueries.DeleteUser(); SqlParameter[] params = { SqlQueries.IdParameter(id) };
        /// </summary>
        public static string DeleteUser()
        {
            return "DELETE FROM Users WHERE Id=@Id";
        }

        /// <summary>
        /// Check if username exists
        /// Usage: string query = SqlQueries.CheckUsernameExists(); SqlParameter[] params = { SqlQueries.UsernameParameter(username) };
        /// </summary>
        public static string CheckUsernameExists()
        {
            return "SELECT COUNT(*) FROM Users WHERE Username=@Username";
        }

        /// <summary>
        /// Validate user login credentials
        /// Usage: string query = SqlQueries.ValidateLogin(); SqlParameter[] params = SqlQueries.LoginParameters(username, password);
        /// </summary>
        public static string ValidateLogin()
        {
            return "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password";
        }

        /// <summary>
        /// Get user count
        /// Usage: int count = (int)DatabaseHelper.ExecuteScalar(SqlQueries.GetUserCount());
        /// </summary>
        public static string GetUserCount()
        {
            return "SELECT COUNT(*) FROM Users";
        }

        /// <summary>
        /// Search users by username (LIKE)
        /// Usage: string query = SqlQueries.SearchUsersByUsername(); SqlParameter[] params = { SqlQueries.SearchParameter(searchTerm) };
        /// </summary>
        public static string SearchUsersByUsername()
        {
            return "SELECT * FROM Users WHERE Username LIKE @Search";
        }

        #endregion

        #region Generic CRUD Query Builders

        /// <summary>
        /// Build SELECT ALL query for any table
        /// Usage: string query = SqlQueries.SelectAll("Products");
        /// </summary>
        public static string SelectAll(string tableName)
        {
            return $"SELECT * FROM {tableName}";
        }

        /// <summary>
        /// Build SELECT by ID query for any table
        /// Usage: string query = SqlQueries.SelectById("Products"); SqlParameter[] params = { SqlQueries.IdParameter(id) };
        /// </summary>
        public static string SelectById(string tableName)
        {
            return $"SELECT * FROM {tableName} WHERE Id=@Id";
        }

        /// <summary>
        /// Build DELETE by ID query for any table
        /// Usage: string query = SqlQueries.DeleteById("Products"); SqlParameter[] params = { SqlQueries.IdParameter(id) };
        /// </summary>
        public static string DeleteById(string tableName)
        {
            return $"DELETE FROM {tableName} WHERE Id=@Id";
        }

        /// <summary>
        /// Build COUNT query for any table
        /// Usage: int count = (int)DatabaseHelper.ExecuteScalar(SqlQueries.CountAll("Products"));
        /// </summary>
        public static string CountAll(string tableName)
        {
            return $"SELECT COUNT(*) FROM {tableName}";
        }

        #endregion

        #region Parameter Helpers

        /// <summary>
        /// Create SqlParameter for Id
        /// Usage: SqlParameter param = SqlQueries.IdParameter(5);
        /// </summary>
        public static SqlParameter IdParameter(int id)
        {
            return new SqlParameter("@Id", id);
        }

        /// <summary>
        /// Create SqlParameter for Username
        /// Usage: SqlParameter param = SqlQueries.UsernameParameter("john");
        /// </summary>
        public static SqlParameter UsernameParameter(string username)
        {
            return new SqlParameter("@Username", username);
        }

        /// <summary>
        /// Create SqlParameter for Password
        /// Usage: SqlParameter param = SqlQueries.PasswordParameter("pass123");
        /// </summary>
        public static SqlParameter PasswordParameter(string password)
        {
            return new SqlParameter("@Password", password);
        }

        /// <summary>
        /// Create SqlParameter for Search (with wildcards)
        /// Usage: SqlParameter param = SqlQueries.SearchParameter("john"); // Becomes '%john%'
        /// </summary>
        public static SqlParameter SearchParameter(string searchTerm)
        {
            return new SqlParameter("@Search", "%" + searchTerm + "%");
        }

        /// <summary>
        /// Create SqlParameter array for User Insert
        /// Usage: SqlParameter[] params = SqlQueries.UserParameters("john", "pass123");
        /// </summary>
        public static SqlParameter[] UserParameters(string username, string password)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
        }

        /// <summary>
        /// Create SqlParameter array for User Update (with Id)
        /// Usage: SqlParameter[] params = SqlQueries.UserUpdateParameters(5, "john", "pass123");
        /// </summary>
        public static SqlParameter[] UserUpdateParameters(int id, string username, string password)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
        }

        /// <summary>
        /// Create SqlParameter array for Login validation
        /// Usage: SqlParameter[] params = SqlQueries.LoginParameters("john", "pass123");
        /// </summary>
        public static SqlParameter[] LoginParameters(string username, string password)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
        }

        #endregion
    }
}
