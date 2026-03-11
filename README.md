# ?? MVC CRUD Management System

ASP.NET MVC application with complete user authentication, session management, and CRUD operations.

## ? Features

- ? **User Authentication** - Login/Register/Logout with session management
- ? **Remember Me** - Cookie-based auto-login functionality
- ? **CRUD Operations** - Complete Create, Read, Update, Delete for users
- ? **Database Helper** - Centralized database connection management
- ? **SQL Queries Helper** - Reusable SQL query methods
- ? **jQuery Validation** - Client-side form validation
- ? **Custom Authorization** - Protected pages with authorization filter
- ? **Beautiful UI** - Bootstrap 5 with custom styling
- ? **Error Handling** - Comprehensive error messages

## ??? Technologies Used

- **Framework:** ASP.NET MVC 5 (.NET Framework 4.7.2)
- **Database:** SQL Server with ADO.NET
- **Frontend:** HTML5, CSS3, Bootstrap 5, jQuery
- **Authentication:** Session & Cookie-based
- **Validation:** jQuery Validation, DataAnnotations

## ?? Project Structure

```
MVC_CRUD_Demo/
??? Controllers/
?   ??? AccountController.cs      # Authentication logic
?   ??? UserController.cs         # CRUD operations
??? Models/
?   ??? DatabaseHelper.cs         # Centralized DB connection
?   ??? SqlQueries.cs             # SQL query helpers
?   ??? UserModel.cs              # User entity
?   ??? LoginViewModel.cs         # Login form
?   ??? RegisterViewModel.cs      # Registration form
??? Views/
?   ??? Account/                  # Login/Register views
?   ??? User/                     # CRUD views
?   ??? Shared/
?       ??? _Layout.cshtml        # Master layout with navbar
??? Filters/
?   ??? CustomAuthorizationFilter.cs
??? Content/                      # CSS files
```

## ?? Getting Started

### Prerequisites

- Visual Studio 2019 or later
- SQL Server 2016 or later
- .NET Framework 4.7.2

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/devpatel22112004/mvc_crud_mangment_systerm.git
   ```

2. **Open the solution**
   - Open `MVC_CRUD_Demo.sln` in Visual Studio

3. **Create the database**
   ```sql
   CREATE DATABASE MVC_CRUD_DB;
   GO
   
   USE MVC_CRUD_DB;
   GO
   
   CREATE TABLE Users (
       Id INT PRIMARY KEY IDENTITY(1,1),
       Username NVARCHAR(50) NOT NULL,
       Password NVARCHAR(100) NOT NULL
   );
   GO
   
   -- Insert test user
   INSERT INTO Users (Username, Password) VALUES ('admin', 'admin123');
   ```

4. **Update connection string**
   - Open `Web.config`
   - Update the connection string if needed:
   ```xml
   <connectionStrings>
       <add name="dbcs"
            connectionString="Data Source=.;Initial Catalog=MVC_CRUD_DB;Integrated Security=True;TrustServerCertificate=True"
            providerName="System.Data.SqlClient"/>
   </connectionStrings>
   ```

5. **Build and Run**
   - Press `F5` or click "Start"
   - Application will open at `http://localhost:port/Account/Login`

## ?? Usage

### Login
- Navigate to `/Account/Login`
- Enter credentials (test user: admin/admin123)
- Check "Remember Me" to save login for 30 days
- Click "Login"

### Register
- Click "Register here" on login page
- Enter username (3-50 chars) and password (6+ chars)
- Submit to create account

### User Management
- View all users at `/User/Index`
- Add new user with "Add New User" button
- Edit user details with "Edit" button
- Delete user with "Delete" button (with confirmation)

## ??? Architecture

### Three-Layer Architecture

```
Controllers (Business Logic)
    ?
SqlQueries (SQL Query Strings)
    ?
DatabaseHelper (Database Execution)
    ?
SQL Server (Database)
```

### Key Components

**DatabaseHelper** - Centralized database operations:
- `ExecuteScalar()` - Get single value
- `ExecuteNonQuery()` - INSERT/UPDATE/DELETE
- `ExecuteReader()` - SELECT multiple rows

**SqlQueries** - Pre-built SQL queries:
- User CRUD operations
- Login validation
- Generic query builders

**CustomAuthorizationFilter** - Protects pages from unauthorized access

## ?? Security Features

- ? Parameterized queries (SQL injection prevention)
- ? Server-side validation (DataAnnotations)
- ? Client-side validation (jQuery)
- ? Anti-forgery tokens (CSRF protection)
- ? Session management
- ? Authorization filter

## ?? Documentation

Complete documentation available in the project:
- `DatabaseHelper_Guide.md` - Complete guide with examples
- `SqlQueries_QuickReference.md` - Quick reference card
- `Architecture_Diagrams.md` - Visual architecture diagrams
- `PROJECT_UPGRADE_SUMMARY.md` - Project summary

## ?? Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## ?? License

This project is open source and available under the [MIT License](LICENSE).

## ?? Author

**Dev Patel**
- GitHub: [@devpatel22112004](https://github.com/devpatel22112004)

## ?? Acknowledgments

- ASP.NET MVC framework
- Bootstrap for UI components
- jQuery for client-side validation

---

**? If you found this project helpful, please give it a star!**
