using RepoDb.Interfaces;
using RepoDb;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Data.SQLite;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;

namespace MinimumApi.Extensions
{
  public static class DbExtensions
  {
    public static TEntity QuerySingle<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> where, IEnumerable<Field> fields = null, IEnumerable<OrderField> orderBy = null, int? top = 0, string hints = null, string cacheKey = null, int? cacheItemExpiration = 180, int? commandTimeout = null, string traceKey = "Query", IDbTransaction transaction = null, ICache cache = null, ITrace trace = null, IStatementBuilder statementBuilder = null) where TEntity : class
        => connection.Query<TEntity>(
            ClassMappedNameCache.Get<TEntity>(),
            where,
            fields,
            orderBy,
            top,
            hints,
            cacheKey,
            cacheItemExpiration,
            commandTimeout,
            traceKey,
            transaction,
            cache,
            trace,
            statementBuilder).FirstOrDefault();

    public static async Task<TEntity> QuerySingleAsync<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> where, IEnumerable<Field> fields = null, IEnumerable<OrderField> orderBy = null, int? top = 0, string hints = null, string cacheKey = null, int? cacheItemExpiration = 180, int? commandTimeout = null, string traceKey = "Query", IDbTransaction transaction = null, ICache cache = null, ITrace trace = null, IStatementBuilder statementBuilder = null, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class
    {
      var result = await connection.QueryAsync<TEntity>(ClassMappedNameCache.Get<TEntity>(), where, fields, orderBy, top, hints, cacheKey, cacheItemExpiration, commandTimeout, traceKey, transaction, cache, trace, statementBuilder, cancellationToken);
      return result.FirstOrDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="initScriptFileName">script file default to the same directory as the sqlite database file directory.</param>
    public static void CreateSQLiteDatabaseIfNotExists(this WebApplication app, string connectionString, string? initScriptFileName = null)
    {
      var csBuilder = new SQLiteConnectionStringBuilder(connectionString);
      if (File.Exists(csBuilder.DataSource)) return;

      SQLiteConnection.CreateFile(csBuilder.DataSource);

      if (initScriptFileName == null) return;
      
      using var scope = app.Services.CreateScope();
      var connection = scope.ServiceProvider.GetService<IDbConnection>();
      if (!File.Exists(initScriptFileName))
        initScriptFileName = Path.Combine(Directory.GetParent(csBuilder.DataSource).FullName, initScriptFileName);

      var script = File.ReadAllText(initScriptFileName);
      connection.ExecuteNonQuery(script);
      
    }

    public static void CreateSqlServerDatabaseIfNotExists(this WebApplication app, string connectionString, string? initScriptFileName = null)
    {
      //using var scope = app.Services.CreateScope();
      var csBuilder = new SqlConnectionStringBuilder(connectionString);
      //var csServerBuilder = new SqlConnectionStringBuilder();
      //csServerBuilder.DataSource = csBuilder.DataSource;
      //csServerBuilder.UserID = csBuilder.UserID;
      //csServerBuilder.Password = csBuilder.Password;
      //csServerBuilder.TrustServerCertificate = csBuilder.TrustServerCertificate;

      //using var serverConnection = new SqlConnection(csServerBuilder.ConnectionString);

      //var sql = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", csBuilder.InitialCatalog);
      //var exists = serverConnection.ExecuteScalar<int>(sql) > 0;
      //if (exists) return;

      //var sqlCreate = $"CREATE DATABASE [{csBuilder.InitialCatalog}]";
      //serverConnection.ExecuteNonQuery(sql);

      //// serverConnection.ExecuteNonQuery($"CREATE LOGIN {csBuilder.UserID} WITH PASSWORD = '{csBuilder.Password}'");
      //var createUserSql = $@"     
      //  IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{csBuilder.UserID}')
      //  BEGIN
      //      CREATE USER {csBuilder.UserID} FOR LOGIN {csBuilder.UserID}
      //      EXEC sp_addrolemember N'db_owner', N'{csBuilder.UserID}'
      //      EXEC master..sp_addsrvrolemember @loginame = N'{csBuilder.UserID}', @rolename = N'sysadmin'
      //  END;
      //";

      //serverConnection.ExecuteNonQuery(createUserSql);




      CreateNewDatabaseSqlserver(csBuilder.DataSource, csBuilder.InitialCatalog, "sa", "password123");
      CreateLoginInSqlServer(csBuilder.DataSource, "sa", "password123", csBuilder.UserID, "password123");
      CreateUserInDatabase(csBuilder.DataSource, "sa", "password123", csBuilder.InitialCatalog, "User1", csBuilder.UserID);
      //AddUserToRoles(csBuilder.DataSource, "sa", "password123", csBuilder.InitialCatalog, "db_datareader", "User1");
      //AddUserToRoles(csBuilder.DataSource, "sa", "password123", csBuilder.InitialCatalog, "db_datawriter", "User1");
      AddUserToRoles(csBuilder.DataSource, "sa", "password123", csBuilder.InitialCatalog, "db_owner", "User1");
      using var connection = new SqlConnection(connectionString);
      if (initScriptFileName == null) return;

      var script = File.ReadAllText(initScriptFileName);
      connection.ExecuteNonQuery(script);
    }

    #region SQL-SERVER-FUNCTION
    /// <summary>
    /// Assign User in to specific role for SQL server database
    /// </summary>
    /// <param name="Server">Server address - example: yourServer.cloudapp.net,1433</param>         
    /// <param name="UserID">User name with sysadmin role</param>
    /// <param name="Database">User database</param>
    /// <param name="Role">New role for this user</param>
    /// <param name="UserToRole">User assigned to new role</param>   
    public static void AddUserToRoles(string Server, string UserID, string Password, string Database, string Role, string UserToRole)
    {
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "TrustServerCertificate=True;SERVER = " + Server + "; DATABASE = " + Database + " ; User ID = " + UserID + "; Pwd = " + Password;
      string sqlCreateDBQuery = " EXEC sp_addrolemember '" + Role + "', " + UserToRole;
      SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, connection);
      try
      {
        connection.Open();
        myCommand.ExecuteNonQuery();
      }
      finally
      {
        connection.Close();
      }
      return;
    }

    /// <summary>
    /// Create new User in specified database base on Login in SQL server
    /// </summary>
    /// <param name="Server">Server address - example: yourServer.cloudapp.net,1433</param>         
    /// <param name="UserID">User name with sysadmin role</param>
    /// <param name="Database">Database for created user</param>
    /// <param name="NewUser">New user Name</param>
    /// <param name="FromLogin">Create user base on this SQL server login</param>   
    public static void CreateUserInDatabase(string Server, string UserID, string Password, string Database, string NewUser, string FromLogin)
    {
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "TrustServerCertificate=True;SERVER = " + Server + "; DATABASE = " + Database + " ; User ID = " + UserID + "; Pwd = " + Password;
      string sqlCreateDBQuery = "CREATE USER " + NewUser + " FROM LOGIN " + FromLogin;
      SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, connection);
      try
      {
        connection.Open();
        myCommand.ExecuteNonQuery();
      }
      finally
      {
        connection.Close();
      }
      return;
    }

    /// <summary>
    /// Create new Login in SQL server
    /// </summary>
    /// <param name="Server">Server address - example: yourServer.cloudapp.net,1433</param>         
    /// <param name="UserID">User name with sysadmin role</param>
    /// <param name="Password">Sysadmin user password</param>
    /// <param name="NewLoginName">New Login Name</param>
    /// <param name="NewLoginPassword">Password for new Login</param>              
    public static void CreateLoginInSqlServer(string Server, string UserID, string Password, string NewLoginName, string NewLoginPassword)
    {
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "TrustServerCertificate=True;SERVER = " + Server + "; DATABASE = master; User ID = " + UserID + "; Pwd = " + Password;
      string sqlCreateDBQuery = "CREATE LOGIN [" + NewLoginName + "] WITH PASSWORD='" + NewLoginPassword + "' " +
          ", CHECK_POLICY=OFF, DEFAULT_DATABASE=minimalApi, DEFAULT_LANGUAGE=[English];";
      SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, connection);
      try
      {
        connection.Open();
        myCommand.ExecuteNonQuery();
      }
      catch { }
      finally
      {
        connection.Close();
      }
      return;
    }


    /// <summary>
    /// Create new empty database on SQL server base on model database stored in SQL server
    /// </summary>
    /// <param name="Server">Server address - example: yourServer.cloudapp.net,1433</param>
    /// <param name="NewDatabaseName">Name for new database</param>
    /// <param name="UserID">User name with sysadmin role</param>
    /// <param name="Password">Sysadmin user password</param>
    public static void CreateNewDatabaseSqlserver(string Server, string NewDatabaseName, string UserID, string Password)
    {
      string sqlCreateDBQuery = " CREATE DATABASE " + NewDatabaseName;
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "TrustServerCertificate=True;SERVER = " + Server + "; DATABASE = master; User ID = " + UserID + "; Pwd = " + Password;
      SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, connection);
      try
      {
        connection.Open();
        myCommand.ExecuteNonQuery();
      }
      finally
      {
        connection.Close();
      }
      return;
    }
    #endregion
  }
}
