using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AegisLabsExam.Common;

/// <summary>
/// Converts <see cref="Pbkdf2Hash"/> to and from a string representation.
/// </summary>
/// <remarks>
/// This converter allows storing hashed passwords in an Entity Framework database.
/// </remarks>
/// <example>
/// Example usage with Entity Framework:
/// <code>
/// modelBuilder.Entity&lt;UserModel&gt;()
///     .Property(e => e.Password)
///     .HasConversion(new Pdkf2HashConverter());
/// </code>
/// </example>
public class Pbkdf2HashConverter() : ValueConverter<Pbkdf2Hash, string>(hash => hash.Value, value => new Pbkdf2Hash(value));