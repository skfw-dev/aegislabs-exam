using AegisLabsExam.Common;
using AegisLabsExam.Helpers;
using AegisLabsExam.Repositories;
using AegisLabsExam.Schemas;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AegisLabsExam.Tests;

public class ExampleTests
{
    private readonly ITestOutputHelper _output;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleTests"/> class.
    /// This constructor sets up the test output helper and enables real-time console output.
    /// </summary>
    /// <param name="output">An instance of <see cref="ITestOutputHelper"/> used for test output.</param>
    public ExampleTests(ITestOutputHelper output)
    {
        // Enable real-time console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        _output = output;
    }

    /// <summary>
    /// Tests the <see cref="TransformText.ToUpperSnakeCase"/> method by providing the string "hello world" and verifying that the output is "HELLO_WORLD".
    /// </summary>
    [Fact]
    public void Test1_TransformText_ToUpperSnakeCase()
    {
        const string input = "hello world";
        var output = TransformText.ToUpperSnakeCase(input);
        Assert.Equal("HELLO_WORLD", output);
    }

    /// <summary>
    /// Tests the <see cref="Pbkdf2Hash.ComparePassword"/> method by hashing a password and then comparing it to the original password and a wrong password.
    /// </summary>
    [Fact]
    public void Test2_Pbkdf2Hash_ComparePassword()
    {
        const string password = "this is a password";
        Pbkdf2Hash hash = password;

        Assert.StartsWith("pbkdf2_", hash.Value);
        Assert.StartsWith("pbkdf2_", (string)hash);
        Assert.True(hash.ComparePassword(password));
        Assert.False(hash.ComparePassword("wrong password"));
    }

    [Fact]
    public void Test3()
    {
        _output.WriteLine("Test 1");
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var connection = config["ConnectionStrings:DefaultConnection"]!;
        _output.WriteLine($"Connection (Default): {connection}");

        var dbHelper = new DatabaseHelper(connection);
        var personRepository = new PersonRepository(dbHelper);
        
        var dataTable = dbHelper.FetchAll("SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');");
        _output.WriteLine(dataTable.FirstAtColumn<string>("name"));

        personRepository.CreateTable();
        
        const string ascii = "abcdefghijklmnopqrstuvwxyz" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "0123456789";
        
        var names = new[]
        {
            "James", 
            "Emma", 
            "Oliver", 
            "Sophia", 
            "William", 
            "Isabella", 
            "Benjamin", 
            "Charlotte", 
            "Henry", 
            "Amelia"
        };
        
        var random = new Random();

        foreach (var name in names)
        {
            var id = new string(Enumerable.Range(0, 8)
                .Select(_ => ascii[random.Next(ascii.Length)])
                .ToArray());

            var age = random.Next(18, 60);

            var person = new Person
            {
                Id = id,
                Name = name,
                Age = age,
            };

            var parameters = new SqlParameter[]
            {
                new("@name", name),
            };
            
            personRepository.Save(person, "name = @name", parameters);
            _output.WriteLine($"Save: {name}");
        }

        var persons = personRepository.FindAll();
        foreach (var person in persons)
        {
            _output.WriteLine($"Id: {person.Id}, Name: {person.Name}, Age: {person.Age}");
        }
        
        // personRepository.DropTable();
    }
}