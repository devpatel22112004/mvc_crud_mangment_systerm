using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using MVC_CRUD_Demo.Models;
using MVC_CRUD_Demo.Filters;

namespace MVC_CRUD_Demo.Controllers
{
    [CustomAuthorizationFilter]
    public class UserController : Controller
    {
        // OLD WAY - No longer needed! DatabaseHelper handles this centrally
        // string cs = ConfigurationManager.ConnectionStrings["dbcs"]?.ConnectionString;

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return View(user);
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return View(user);
                }

                // OLD WAY - Manual query
                // string query = "INSERT INTO Users VALUES(@Username,@Password)";
                // SqlParameter[] parameters = {
                //     new SqlParameter("@Username", user.Username),
                //     new SqlParameter("@Password", user.Password)
                // };

                // NEWEST WAY - Using SqlQueries helper
                string query = SqlQueries.InsertUser();
                SqlParameter[] parameters = SqlQueries.UserParameters(user.Username, user.Password);
                
                DatabaseHelper.ExecuteNonQuery(query, parameters);

                TempData["Success"] = "User created successfully";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View(user);
            }
        }

        public ActionResult Index()
        {
            List<UserModel> users = new List<UserModel>();

            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return View(users);
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return View(users);
                }

                // OLD WAY - Manual query
                // string query = "SELECT * FROM Users";

                // NEWEST WAY - Using SqlQueries helper
                string query = SqlQueries.GetAllUsers();
                
                using (SqlDataReader rd = DatabaseHelper.ExecuteReader(query))
                {
                    while (rd.Read())
                    {
                        UserModel user = new UserModel();
                        user.Id = Convert.ToInt32(rd["Id"]);
                        user.Username = rd["Username"].ToString();
                        user.Password = rd["Password"].ToString();
                        users.Add(user);
                    }
                }
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View(users);
        }

        public ActionResult Edit(int id)
        {
            UserModel user = new UserModel();

            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return View(user);
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return View(user);
                }

                // OLD WAY - Manual query
                // string query = "SELECT * FROM Users WHERE Id=@Id";
                // SqlParameter[] parameters = { new SqlParameter("@Id", id) };

                // NEWEST WAY - Using SqlQueries helper
                string query = SqlQueries.GetUserById();
                SqlParameter[] parameters = { SqlQueries.IdParameter(id) };

                using (SqlDataReader rd = DatabaseHelper.ExecuteReader(query, parameters))
                {
                    if (rd.Read())
                    {
                        user.Id = Convert.ToInt32(rd["Id"]);
                        user.Username = rd["Username"].ToString();
                        user.Password = rd["Password"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return View(user);
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return View(user);
                }

                // OLD WAY - Manual query
                // string query = "UPDATE Users SET Username=@Username,Password=@Password WHERE Id=@Id";
                // SqlParameter[] parameters = {
                //     new SqlParameter("@Username", user.Username),
                //     new SqlParameter("@Password", user.Password),
                //     new SqlParameter("@Id", user.Id)
                // };

                // NEWEST WAY - Using SqlQueries helper
                string query = SqlQueries.UpdateUser();
                SqlParameter[] parameters = SqlQueries.UserUpdateParameters(user.Id, user.Username, user.Password);
                
                DatabaseHelper.ExecuteNonQuery(query, parameters);

                TempData["Success"] = "User updated successfully";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View(user);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return RedirectToAction("Index");
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return RedirectToAction("Index");
                }

                // OLD WAY - Manual query
                // string query = "DELETE FROM Users WHERE Id=@Id";
                // SqlParameter[] parameters = { new SqlParameter("@Id", id) };

                // NEWEST WAY - Using SqlQueries helper
                string query = SqlQueries.DeleteUser();
                SqlParameter[] parameters = { SqlQueries.IdParameter(id) };
                
                DatabaseHelper.ExecuteNonQuery(query, parameters);

                TempData["Success"] = "User deleted successfully";
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}