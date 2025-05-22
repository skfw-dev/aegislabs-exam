using System.Data;
using AegisLabsExam.Helpers;
using AegisLabsExam.Schemas;
using Microsoft.Data.SqlClient;

namespace AegisLabsExam.Repositories;

public interface IPersonRepository
{
    public void CreateTable();
    public Task CreateTableAsync(CancellationToken cancellationToken = default);
    public void DropTable();
    public Task DropTableAsync(CancellationToken cancellationToken = default);
    public int Add(Person person);
    public Task<int> AddAsync(Person person, CancellationToken cancellationToken = default);
    public Person First(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null);
    public Task<Person> FirstAsync(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public Person? FirstOrDefault(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null);
    public Task<Person?> FirstOrDefaultAsync(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public IEnumerable<Person> FindAll(string whereClause = "deleted_at IS NULL", SqlParameter[]? parameters = null);
    public Task<IEnumerable<Person>> FindAllAsync(string whereClause = "deleted_at IS NULL", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public bool Check(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null);
    public Task<bool> CheckAsync(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public int Save(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null);
    public Task<int> SaveAsync(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public int Update(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null);
    public Task<int> UpdateAsync(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
    public int Delete(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null);
    public Task<int> DeleteAsync(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default);
}

public sealed class PersonRepository(IDatabaseHelper dbHelper) : IPersonRepository
{
    private IDatabaseHelper DbHelper => dbHelper;

    /// <summary>
    /// Creates a person's table in the database if it does not already exist.
    /// </summary>
    /// <remarks>
    /// The table created will have the following columns:
    /// <list type="bullet">
    ///     <item>
    ///         <term>id</term>
    ///         <description> - Primary key. The ID of the person.</description>
    ///     </item>
    ///     <item>
    ///         <term>name</term>
    ///         <description> - The name of the person.</description>
    ///     </item>
    ///     <item>
    ///         <term>age</term>
    ///         <description> - The age of the person.</description>
    ///     </item>
    ///     <item>
    ///         <term>created_at</term>
    ///         <description> - 
    ///             The time at which the person was created.
    ///             Defaults to the current time if not specified.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>updated_at</term>
    ///         <description> - 
    ///             The time at which the person was most recently updated.
    ///             Defaults to the current time if not specified.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>deleted_at</term>
    ///         <description> - 
    ///             The time at which the person was deleted.
    ///             Null if the person has not been deleted.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    public void CreateTable()
    {
        const string query = """
                               IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'persons')
                               BEGIN
                                   CREATE TABLE dbo.persons (
                                       id VARCHAR(8) PRIMARY KEY,
                                       name VARCHAR(255),
                                       age INT,
                                       created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
                                       updated_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
                                       deleted_at DATETIMEOFFSET NULL
                                   );
                               END;
                               """;
        
        DbHelper.ExecuteSql(query);
    }
    
    /// <summary>
    /// Asynchronously creates a table named 'dbo.persons' in the database if it does not already exist.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <remarks>
    /// The table created will have the following columns:
    /// <list type="bullet">
    ///     <item>
    ///         <term>id</term>
    ///         <description> - Primary key. The ID of the person.</description>
    ///     </item>
    ///     <item>
    ///         <term>name</term>
    ///         <description> - The name of the person.</description>
    ///     </item>
    ///     <item>
    ///         <term>age</term>
    ///         <description> - The age of the person.</description>
    ///     </item>
    ///     <item>
    ///         <term>created_at</term>
    ///         <description> - 
    ///             The time at which the person was created.
    ///             Defaults to the current time if not specified.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>updated_at</term>
    ///         <description> - 
    ///             The time at which the person was most recently updated.
    ///             Defaults to the current time if not specified.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>deleted_at</term>
    ///         <description> - 
    ///             The time at which the person was deleted.
    ///             Null if the person has not been deleted.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    public async Task CreateTableAsync(CancellationToken cancellationToken = default)
    {
        const string query = """
                               IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'persons')
                               BEGIN
                                   CREATE TABLE dbo.persons (
                                       id VARCHAR(8) PRIMARY KEY,
                                       name VARCHAR(255),
                                       age INT,
                                       created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
                                       updated_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
                                       deleted_at DATETIMEOFFSET NULL
                                   );
                               END;
                               """;
        
        await DbHelper.ExecuteSqlAsync(query, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Drops the 'dbo.persons' table from the database if it exists.
    /// </summary>
    /// <remarks>
    /// This method will execute a SQL command to remove the 'persons' table,
    /// which contains data related to persons, from the database.
    /// </remarks>
    public void DropTable()
    {
        const string query = "DROP TABLE IF EXISTS dbo.persons";
        DbHelper.ExecuteSql(query);
    }
    
    /// <summary>
    /// Asynchronously drops the 'dbo.persons' table from the database if it exists.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <remarks>
    /// This method will execute a SQL command to remove the 'persons' table,
    /// which contains data related to persons, from the database.
    /// </remarks>
    public async Task DropTableAsync(CancellationToken cancellationToken = default)
    {
        const string query = "DROP TABLE IF EXISTS dbo.persons";
        await DbHelper.ExecuteSqlAsync(query, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Adds a person to the database.
    /// </summary>
    /// <param name="person">The person to add.</param>
    /// <returns>The number of records affected.</returns>
    public int Add(Person person)
    {
        const string query = "INSERT INTO dbo.persons (id, name, age) VALUES (@id, @name, @age)";
        var parameters = new SqlParameter[]
        {
            new("@id", person.Id),
            new("@name", person.Name),
            new("@age", person.Age),
        };
        var results = DbHelper.ExecuteSql(query, parameters);
        return results.RecordsAffected;
    }
    
    /// <summary>
    /// Asynchronously adds a person to the database.
    /// </summary>
    /// <param name="person">The person to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of records affected.</returns>
    public async Task<int> AddAsync(Person person, CancellationToken cancellationToken = default)
    {
        const string query = "INSERT INTO dbo.persons (id, name, age) VALUES (@id, @name, @age)";
        var parameters = new SqlParameter[]
        {
            new("@id", person.Id),
            new("@name", person.Name),
            new("@age", person.Age),
        };
        var results = await DbHelper.ExecuteSqlAsync(query, parameters, cancellationToken: cancellationToken);
        return results.RecordsAffected;
    }
    
    /// <summary>
    /// Retrieves the first person from the database that matches the given criteria.
    /// </summary>
    /// <param name="person">The person to search for.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>The first matching person, or the default value of <see cref="Person"/>.</returns>
    public Person First(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null)
    {
        var query = $"SELECT TOP 1 id, name, age AS count FROM dbo.persons WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = DbHelper.FetchAll(query, parameters);
        return results
            .Convert(row => new Person
            {
                Id = row.Field<string>("id")!,
                Name = row.Field<string>("name")!,
                Age = row.Field<int>("age")
            })
            .First();
    }
    
    /// <summary>
    /// Asynchronously retrieves the first person from the database that matches the given criteria.
    /// </summary>
    /// <param name="person">The person to search for.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The first matching person.</returns>
    public async Task<Person> FirstAsync(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT TOP 1 id, name, age AS count FROM dbo.persons WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = await DbHelper.FetchAllAsync(query, parameters, cancellationToken: cancellationToken);
        return results
            .Convert(row => new Person
            {
                Id = row.Field<string>("id")!,
                Name = row.Field<string>("name")!,
                Age = row.Field<int>("age")
            })
            .First();
    }
    
    /// <summary>
    /// Retrieves the first person from the database that matches the given criteria, or the default value of <see cref="Person"/>.
    /// </summary>
    /// <param name="person">The person to search for.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>The first matching person, or the default value of <see cref="Person"/>.</returns>
    public Person? FirstOrDefault(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null)
    {
        var query = $"SELECT TOP 1 id, name, age AS count FROM dbo.persons WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = DbHelper.FetchAll(query, parameters);
        return results
            .Convert(row => new Person
            {
                Id = row.Field<string>("id")!,
                Name = row.Field<string>("name")!,
                Age = row.Field<int>("age")
            })
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Asynchronously retrieves the first person from the database that matches the given criteria, or the default value of <see cref="Person"/>.
    /// </summary>
    /// <param name="person">The person to search for.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The first matching person, or the default value of <see cref="Person"/>.</returns>
    public async Task<Person?> FirstOrDefaultAsync(Person? person, string? whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT TOP 1 id, name, age AS count FROM dbo.persons WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = await DbHelper.FetchAllAsync(query, parameters, cancellationToken: cancellationToken);
        return results
            .Convert(row => new Person
            {
                Id = row.Field<string>("id")!,
                Name = row.Field<string>("name")!,
                Age = row.Field<int>("age")
            })
            .FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the persons from the database that match the given criteria.
    /// </summary>
    /// <param name="whereClause">The WHERE clause to apply to the query.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>The persons that match the given criteria.</returns>
    public IEnumerable<Person> FindAll(string whereClause = "deleted_at IS NULL", SqlParameter[]? parameters = null)
    {
        var query = $"SELECT id, name, age FROM dbo.persons WHERE {whereClause}";
        var results = DbHelper.FetchAll(query, parameters);
        return results
            .Convert(row => new Person
            {
                Id = row.Field<string>("id")!,
                Name = row.Field<string>("name")!,
                Age = row.Field<int>("age"),
            });
    }
    
    /// <summary>
    /// Asynchronously retrieves the persons from the database that match the given criteria.
    /// </summary>
    /// <param name="whereClause">The WHERE clause to apply to the query.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The persons that match the given criteria.</returns>
    public async Task<IEnumerable<Person>> FindAllAsync(string whereClause = "deleted_at IS NULL", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT id, name, age FROM dbo.persons WHERE {whereClause}";
        var results = await DbHelper.FetchAllAsync(query, parameters, cancellationToken: cancellationToken);
        return results
            .Convert(row => new Person
            {
                Id = row.Field<string>("id")!,
                Name = row.Field<string>("name")!,
                Age = row.Field<int>("age"),
            });
    }

    /// <summary>
    /// Checks if a person exists in the database based on the given criteria.
    /// </summary>
    /// <param name="person">The person to check for existence.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to checking by ID.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <returns><see langword="true"/> if the person exists in the database; otherwise, <see langword="false"/>.</returns>
    public bool Check(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null)
    {
        var query = $"SELECT TOP 1 COUNT(*) AS count FROM dbo.persons WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = DbHelper.FetchAll(query, parameters);
        return results.Count > 0 && results.FirstAtColumn<int>("count") > 0;
    }
    
    /// <summary>
    /// Asynchronously checks if a person exists in the database based on the given criteria.
    /// </summary>
    /// <param name="person">The person to check for existence.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to checking by ID.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns><see langword="true"/> if the person exists in the database; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> CheckAsync(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT TOP 1 COUNT(*) AS count FROM dbo.persons WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = await DbHelper.FetchAllAsync(query, parameters, cancellationToken: cancellationToken);
        return results.Count > 0 && results.FirstAtColumn<int>("count") > 0;
    }

    /// <summary>
    /// Saves a person to the database. If the person does not yet exist in the database,
    /// it will be added; otherwise, the existing person will be updated.
    /// </summary>
    /// <param name="person">The person to save.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to checking by ID.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <returns>The number of records affected.</returns>
    public int Save(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null)
    {
        var check = Check(person, whereClause, parameters);
        return check ? Update(person, whereClause, parameters) : 
            Add(person);
    }
    
    /// <summary>
    /// Asynchronously saves a person to the database. If the person does not yet exist in the database,
    /// it will be added; otherwise, the existing person will be updated.
    /// </summary>
    /// <param name="person">The person to save.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to checking by ID.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of records affected.</returns>
    public async Task<int> SaveAsync(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var check = await CheckAsync(person, whereClause, parameters, cancellationToken: cancellationToken);
        return check ? await UpdateAsync(person, whereClause, parameters, cancellationToken: cancellationToken) : 
            await AddAsync(person, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Updates the properties of a person in the database.
    /// </summary>
    /// <param name="person">The person to update.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to 'id = @id'.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <returns>The number of records affected.</returns>
    public int Update(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null)
    {
        var query = $"UPDATE dbo.persons SET name = @_name, age = @_age, updated_at = SYSDATETIMEOFFSET() WHERE {whereClause}";
        parameters ??= [ new SqlParameter("@id", person.Id) ];
        var values = new SqlParameter[]
        {
            new("@_name", person.Name),
            new("@_age", person.Age),
        };
        parameters = values.Concat(parameters).ToArray();
        var results = DbHelper.ExecuteSql(query, parameters);
        return results.RecordsAffected;
    }

    /// <summary>
    /// Asynchronously updates the properties of a person in the database.
    /// </summary>
    /// <param name="person">The person to update.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to 'id = @id'.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of records affected.</returns>
    public async Task<int> UpdateAsync(Person person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var query = $"UPDATE dbo.persons SET name = @_name, age = @_age, updated_at = SYSDATETIMEOFFSET() WHERE {whereClause}";
        parameters ??= [ new SqlParameter("@id", person.Id) ];
        var values = new SqlParameter[]
        {
            new("@_name", person.Name),
            new("@_age", person.Age),
        };
        parameters = values.Concat(parameters).ToArray();
        var results = await DbHelper.ExecuteSqlAsync(query, parameters, cancellationToken: cancellationToken);
        return results.RecordsAffected;
    }
    
    /// <summary>
    /// Marks a person as deleted in the database by setting the 'deleted_at' timestamp.
    /// </summary>
    /// <param name="person">The person to mark as deleted.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to 'id = @id'.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <returns>The number of records affected.</returns>
    public int Delete(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null)
    {
        var query = $"UPDATE dbo.persons SET deleted_at = SYSDATETIMEOFFSET() WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = DbHelper.ExecuteSql(query, parameters);
        return results.RecordsAffected;
    }
    
    /// <summary>
    /// Asynchronously marks a person as deleted in the database by setting the 'deleted_at' timestamp.
    /// </summary>
    /// <param name="person">The person to mark as deleted.</param>
    /// <param name="whereClause">The WHERE clause to apply to the query. Defaults to 'id = @id'.</param>
    /// <param name="parameters">The parameters to pass to the query. Defaults to the person's ID.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of records affected.</returns>
    public async Task<int> DeleteAsync(Person? person, string whereClause = "id = @id", SqlParameter[]? parameters = null, CancellationToken cancellationToken = default)
    {
        var query = $"UPDATE dbo.persons SET deleted_at = SYSDATETIMEOFFSET() WHERE {whereClause}";
        parameters ??= person is not null ? [ new SqlParameter("@id", person.Id) ] : [];
        var results = await DbHelper.ExecuteSqlAsync(query, parameters, cancellationToken: cancellationToken);
        return results.RecordsAffected;
    }
}
