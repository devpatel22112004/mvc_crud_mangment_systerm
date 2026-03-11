using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using MVC_CRUD_Demo.Models;

namespace MVC_CRUD_Demo.Controllers
{
    public class AccountController : Controller
    {
        // OLD WAY - No longer needed! DatabaseHelper handles this centrally
        // string cs = ConfigurationManager.ConnectionStrings["dbcs"]?.ConnectionString;

        // GET: Login
        public ActionResult Login()
        {
            // If already logged in, redirect to User/Index
            if (Session["Username"] != null)
            {
                return RedirectToAction("Index", "User");
            }

            // Check if "Remember Me" cookie exists
            if (Request.Cookies["Username"] != null && Request.Cookies["Password"] != null)
            {
                var model = new LoginViewModel
                {
                    Username = Request.Cookies["Username"].Value,
                    Password = Request.Cookies["Password"].Value,
                    RememberMe = true
                };
                return View(model);
            }

            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return View(model);
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return View(model);
                }

                // OLD WAY - Manual query and parameters
                // string query = "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password";
                // SqlParameter[] parameters = {
                //     new SqlParameter("@Username", model.Username),
                //     new SqlParameter("@Password", model.Password)
                // };

                // NEWEST WAY - Using SqlQueries helper
                string query = SqlQueries.ValidateLogin();
                SqlParameter[] parameters = SqlQueries.LoginParameters(model.Username, model.Password);

                int count = (int)DatabaseHelper.ExecuteScalar(query, parameters);

                if (count > 0)
                {
                    // LOGIN SUCCESSFUL - Create Session
                    Session["Username"] = model.Username;
                    Session["IsLoggedIn"] = true;
                    Session.Timeout = 30; // Session timeout in minutes

                    // If "Remember Me" is checked, create cookies
                    if (model.RememberMe)
                    {
                        HttpCookie usernameCookie = new HttpCookie("Username", model.Username);
                        HttpCookie passwordCookie = new HttpCookie("Password", model.Password);

                        // Set cookie expiry for 30 days
                        usernameCookie.Expires = DateTime.Now.AddDays(30);
                        passwordCookie.Expires = DateTime.Now.AddDays(30);

                        Response.Cookies.Add(usernameCookie);
                        Response.Cookies.Add(passwordCookie);
                    }
                    else
                    {
                        // Remove cookies if "Remember Me" is not checked
                        if (Request.Cookies["Username"] != null)
                        {
                            HttpCookie usernameCookie = new HttpCookie("Username");
                            usernameCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(usernameCookie);
                        }

                        if (Request.Cookies["Password"] != null)
                        {
                            HttpCookie passwordCookie = new HttpCookie("Password");
                            passwordCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(passwordCookie);
                        }
                    }

                    TempData["Success"] = "Login successful! Welcome " + model.Username;
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["Error"] = "Invalid username or password";
                    return View(model);
                }
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View(model);
            }
        }

        // GET: Register
        public ActionResult Register()
        {
            // If already logged in, redirect to User/Index
            if (Session["Username"] != null)
            {
                return RedirectToAction("Index", "User");
            }

            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // OLD WAY - Individual connection string check
                // if (string.IsNullOrEmpty(cs))
                // {
                //     TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                //     return View(model);
                // }

                // NEW WAY - Using DatabaseHelper
                if (!DatabaseHelper.IsConnectionStringValid())
                {
                    TempData["Error"] = "Connection string 'dbcs' not found in Web.config";
                    return View(model);
                }

                // OLD WAY - Manual query
                // string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username=@Username";
                // SqlParameter[] checkParams = { new SqlParameter("@Username", model.Username) };

                // NEWEST WAY - Using SqlQueries helper
                string checkQuery = SqlQueries.CheckUsernameExists();
                SqlParameter[] checkParams = { SqlQueries.UsernameParameter(model.Username) };
                
                int count = (int)DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (count > 0)
                {
                    TempData["Error"] = "Username already exists. Please choose a different username.";
                    return View(model);
                }

                // OLD WAY - Manual INSERT query
                // string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                // SqlParameter[] insertParams = {
                //     new SqlParameter("@Username", model.Username),
                //     new SqlParameter("@Password", model.Password)
                // };

                // NEWEST WAY - Using SqlQueries helper
                string insertQuery = SqlQueries.InsertUser();
                SqlParameter[] insertParams = SqlQueries.UserParameters(model.Username, model.Password);
                
                DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);

                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database Error: " + ex.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View(model);
            }
        }

        // Logout
        public ActionResult Logout()
        {
            // Clear Session
            Session["Username"] = null;
            Session["IsLoggedIn"] = null;
            Session.Clear();
            Session.Abandon();

            // Clear cookies
            if (Request.Cookies["Username"] != null)
            {
                HttpCookie usernameCookie = new HttpCookie("Username");
                usernameCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(usernameCookie);
            }

            if (Request.Cookies["Password"] != null)
            {
                HttpCookie passwordCookie = new HttpCookie("Password");
                passwordCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(passwordCookie);
            }

            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}
