using System.Data;
using System.Text.Json.Serialization;
using AegisLabsExam.Helpers;
using Microsoft.Data.SqlClient;

namespace AegisLabsExam.Schemas;

public class Person
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("age")]
    public int Age { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Person))]
public partial class PersonJsonSerializerContext : JsonSerializerContext;

public static class PersonExtensions
{
    /// <summary>
    /// Updates the properties of a <see cref="Person"/> instance.
    /// </summary>
    /// <param name="person">The <see cref="Person"/> instance to update.</param>
    /// <param name="id">The new ID to assign.</param>
    /// <param name="name">The new name to assign.</param>
    /// <param name="age">The new age to assign.</param>
    /// <returns>The updated <see cref="Person"/> instance.</returns>
    public static Person Update(this Person person, string id, string name, int age)
    {
        person.Id = id;
        person.Name = name;
        person.Age = age;
        return person;
    }
}
