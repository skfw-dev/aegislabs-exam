using System.Data;
using Microsoft.Data.SqlClient;

namespace AegisLabsExam.Helpers;

public interface IDatabaseHelperResults
{
    public DataTable? DataTable { get; init; }
    public int RecordsAffected { get; init; }
    public int Count { get; }
    public IEnumerable<T> Convert<T>(Func<DataRow, T> selector);
    public DataRow First();
    public DataRow? FirstOrDefault();
    public T FirstAtColumn<T>(string columnName);
    public T? FirstOrDefaultAtColumn<T>(string columnName);
}

public class DatabaseHelperResults : IDatabaseHelperResults
{
    public DataTable? DataTable { get; init; }
    public int RecordsAffected { get; init; }
    public int Count => DataTable?.Rows.Count ?? 0;
    
    /// <summary>
    /// Converts the records of the DataTable to the given type.
    /// </summary>
    /// <typeparam name="T">The type to which the records should be converted.</typeparam>
    /// <param name="selector">A function that converts a DataRow to the given type.</param>
    /// <returns>The records converted to the given type.</returns>
    public IEnumerable<T> Convert<T>(Func<DataRow, T> selector) => DataTable?.AsEnumerable().Select(selector) ?? Enumerable.Empty<T>();
    
    /// <summary>
    /// Returns the first row of the DataTable.
    /// </summary>
    /// <exception cref="InvalidOperationException">There are no rows in the DataTable.</exception>
    /// <returns>The first row of the DataTable.</returns>
    public DataRow First()
    {
        return DataTable!.Rows.OfType<DataRow>().First();
    }
    
    /// <summary>
    /// Returns the first DataRow of the DataTable if it exists; otherwise, returns null.
    /// </summary>
    /// <returns>The first DataRow if it exists, otherwise null.</returns>
    public DataRow? FirstOrDefault()
    {
        return DataTable?.Rows.OfType<DataRow>().FirstOrDefault();
    }
    
    /// <summary>
    /// Retrieves the value of the specified column from the first DataRow in the DataTable.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="columnName">The name of the column from which to retrieve the value.</param>
    /// <returns>The value from the specified column in the first DataRow.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the DataTable is null or empty.</exception>
    public T FirstAtColumn<T>(string columnName)
    {
        var row = First();
        return row.Field<T>(columnName)!;
    }
    
    /// <summary>
    /// Returns the first record's value of the specified column name if any exists, otherwise the default value.
    /// </summary>
    /// <typeparam name="T">The type of the value to be returned.</typeparam>
    /// <param name="columnName">The name of the column to be retrieved.</param>
    /// <returns>The first record's value of the column if it exists, otherwise the default value.</returns>
    public T? FirstOrDefaultAtColumn<T>(string columnName)
    {
        var row = FirstOrDefault();
        return row is null ? default : row.Field<T>(columnName);
    }
}

public interface IDatabaseHelper
{
    public IDatabaseHelperResults FetchAll(string query, SqlParameter[]? parameters = null);
    public Task<IDatabaseHelperResults> FetchAllAsync(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public IDatabaseHelperResults ExecuteSql(string query, SqlParameter[]? parameters = null);
    public Task<IDatabaseHelperResults> ExecuteSqlAsync(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
}

public class DatabaseHelper : IDatabaseHelper
{
    private readonly string _connection;

    /// <summary>
    /// Initializes a new instance of <see cref="DatabaseHelper"/> with the given connection string.
    /// </summary>
    /// <param name="connection">The connection string to the database.</param>
    public DatabaseHelper(string connection)
    {
        _connection = connection;
    }
    
    /// <summary>
    /// Initializes a new instance of <see cref="DatabaseHelper"/> with a connection string retrieved from the given configuration.
    /// </summary>
    /// <param name="configuration">The configuration from which to retrieve the connection string.</param>
    /// <remarks>
    /// The connection string is retrieved from the configuration using the key "DefaultConnection".
    /// </remarks>
    public DatabaseHelper(IConfiguration configuration)
    {
        _connection = configuration.GetConnectionString("DefaultConnection")!;
    }
    
    /// <summary>
    /// Executes the given Transact-SQL query against the database and returns the results as a DataTable.
    /// </summary>
    /// <param name="query">The Transact-SQL query to execute.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>A <see cref="DatabaseHelperResults"/> object containing a DataTable with the results, and the number of records affected.</returns>
    public IDatabaseHelperResults FetchAll(string query, SqlParameter[]? parameters = null)
    {
        using var conn = new SqlConnection(_connection);
        using var cmd = new SqlCommand(query, conn);
        if (parameters != null)
        {
            parameters = parameters
                .Select(parameter => new SqlParameter(parameter.ParameterName, parameter.Value))
                .ToArray();
            
            cmd.Parameters.AddRange(parameters);
        }
        
        conn.Open();
        using var reader = cmd.ExecuteReader();
        var dataTable = new DataTable();
        dataTable.Load(reader);
        return new DatabaseHelperResults()
        {
            DataTable = dataTable,
            RecordsAffected = reader.RecordsAffected,
        };
    }
    
    /// <summary>
    /// Asynchronously executes the given Transact-SQL query against the database and returns the results as a DataTable.
    /// </summary>
    /// <param name="query">The Transact-SQL query to execute.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DatabaseHelperResults"/> object containing a DataTable with the results and the number of records affected.</returns>
    public async Task<IDatabaseHelperResults> FetchAllAsync(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var conn = new SqlConnection(_connection);
        await using var cmd = new SqlCommand(query, conn);
        if (parameters != null)
        {
            parameters = parameters
                .Select(parameter => new SqlParameter(parameter.ParameterName, parameter.Value))
                .ToArray();
            
            cmd.Parameters.AddRange(parameters);
        }
        
        await conn.OpenAsync(cancellationToken);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var dataTable = new DataTable();
        dataTable.Load(reader);
        return new DatabaseHelperResults()
        {
            DataTable = dataTable,
            RecordsAffected = reader.RecordsAffected,
        };
    }
    
    /// <summary>
    /// Executes the given Transact-SQL statement against the database.
    /// </summary>
    /// <param name="query">The Transact-SQL statement to execute.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>A <see cref="DatabaseHelperResults"/> object containing the number of records affected.</returns>
    public IDatabaseHelperResults ExecuteSql(string query, SqlParameter[]? parameters = null)
    {
        using var conn = new SqlConnection(_connection);
        using var cmd = new SqlCommand(query, conn);
        if (parameters != null)
        {
            parameters = parameters
                .Select(parameter => new SqlParameter(parameter.ParameterName, parameter.Value))
                .ToArray();
            
            cmd.Parameters.AddRange(parameters);
        }
        
        conn.Open();
        var recordAffected = cmd.ExecuteNonQuery();
        return new DatabaseHelperResults()
        {
            RecordsAffected = recordAffected,
        };
    }
    
    /// <summary>
    /// Asynchronously executes the given Transact-SQL statement against the database.
    /// </summary>
    /// <param name="query">The Transact-SQL statement to execute.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="DatabaseHelperResults"/> object containing the number of records affected.</returns>
    public async Task<IDatabaseHelperResults> ExecuteSqlAsync(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var conn = new SqlConnection(_connection);
        await using var cmd = new SqlCommand(query, conn);
        if (parameters != null)
        {
            parameters = parameters
                .Select(parameter => new SqlParameter(parameter.ParameterName, parameter.Value))
                .ToArray();
            
            cmd.Parameters.AddRange(parameters);
        }
        
        await conn.OpenAsync(cancellationToken);
        var recordAffected = await cmd.ExecuteNonQueryAsync(cancellationToken);
        return new DatabaseHelperResults()
        {
            RecordsAffected = recordAffected,
        };
    }
}

public delegate IDatabaseHelperResults DatabaseHelperScriptsExecuteDelegate(SqlParameter[]? parameters = null);
public delegate Task<IDatabaseHelperResults> DatabaseHelperScriptsExecuteAsyncDelegate(SqlParameter[]? parameters = null);

public interface IDatabaseHelperScripts : IDatabaseHelper
{
    public IDatabaseHelper DbHelper { get; }
    public DatabaseHelperScriptsExecuteDelegate Exec(string name, string prefix = "Proc", string separator = "_");
    public DatabaseHelperScriptsExecuteAsyncDelegate ExecAsync(string name, string prefix = "Proc", string separator = "_", CancellationToken cancellationToken = default);
}

public class DatabaseHelperScripts(IDatabaseHelper dbHelper) : IDatabaseHelperScripts
{
    public IDatabaseHelper DbHelper => dbHelper;
    
    public IDatabaseHelperResults FetchAll(string query, SqlParameter[]? parameters = null) 
    {
        return DbHelper.FetchAll(query, parameters);
    }
    
    public async Task<IDatabaseHelperResults> FetchAllAsync(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) 
    {
        return await DbHelper.FetchAllAsync(query, parameters, cancellationToken);
    }
    
    public IDatabaseHelperResults ExecuteSql(string query, SqlParameter[]? parameters = null) 
    {
        return DbHelper.ExecuteSql(query, parameters);
    }
    
    public async Task<IDatabaseHelperResults> ExecuteSqlAsync(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) 
    {
        return await DbHelper.ExecuteSqlAsync(query, parameters, cancellationToken); 
    }
    
    public DatabaseHelperScriptsExecuteDelegate Exec(string name, string prefix = "Proc", string separator = "_")
    {
        var target = string.Join(separator, [prefix, name]);
        return parameters => FetchAll($"EXEC {target}", parameters);
    }

    public DatabaseHelperScriptsExecuteAsyncDelegate ExecAsync(string name, string prefix = "Proc", string separator = "_", CancellationToken cancellationToken = default)
    {
        var target = string.Join(separator, [prefix, name]);
        return async parameters => await FetchAllAsync($"EXEC {target}", parameters, cancellationToken);
    }
}

public class DatabaseHelperOptions
{
    public List<string> Scripts { get; set; } = [];
}

public delegate void DatabaseHelperConfigureDelegate(DatabaseHelperOptions options); 

public static class DatabaseHelperExtensions
{ 
    /// <summary>
    /// Adds a database helper to the service collection and configures it
    /// with the given <paramref name="configure"/> delegate.
    /// </summary>
    /// <remarks>
    /// The <paramref name="configure"/> delegate is called to configure the
    /// <see cref="DatabaseHelperOptions"/> which is used to setup the
    /// database helper scripts.
    /// 
    /// The database helper scripts are executed when the
    /// <see cref="UseDatabaseHelperScripts"/> middleware is used in the
    /// application or when any controller needs an instance of
    /// <see cref="IDatabaseHelperScripts"/>.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">A delegate that configures the database helper options.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDatabaseHelper(this IServiceCollection services, DatabaseHelperConfigureDelegate configure)
    {
        var options = new DatabaseHelperOptions();
        services.AddSingleton<IDatabaseHelper, DatabaseHelper>();
        configure.Invoke(options);
        
        services.AddSingleton<IDatabaseHelperScripts>(serviceProvider =>
        {
            // Not executed until some controller needs it, or use `UseDatabaseHelperScripts` for triggering
            var dbHelper = serviceProvider.GetRequiredService<IDatabaseHelper>();
            foreach (var query in options.Scripts.Select(File.ReadAllText))
            {
                dbHelper.ExecuteSql(query);
            }

            return new DatabaseHelperScripts(dbHelper);
        });
        
        return services;
    }
    
    /// <summary>
    /// Executes database helper scripts during the application startup.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IApplicationBuilder"/>.</returns>
    /// <remarks>
    /// This method retrieves and executes the registered database helper scripts 
    /// upon application startup. It ensures that any scripts configured with 
    /// <see cref="AddDatabaseHelper"/> are executed when the application begins.
    /// </remarks>
    public static IApplicationBuilder UseDatabaseHelperScripts(this IApplicationBuilder app)
    {
        // Trigger the database helper execution scripts on startup
        app.ApplicationServices.GetRequiredService<IDatabaseHelperScripts>();
        return app;
    }
}
