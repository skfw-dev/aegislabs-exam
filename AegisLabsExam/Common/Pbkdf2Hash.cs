using System.Security.Cryptography;

namespace AegisLabsExam.Common;

public class Pbkdf2Hash(string password)
{
    private const int SaltSize = 16;  // Recommended salt size
    private const int HashSize = 32;  // Recommended hash size
    private const int Iterations = 10000;  // Recommended iterations
    private const string Prefix = "pbkdf2_";  // Pattern prefix for easy identification

    public string Value { get; } = HashPassword(password);

    public static implicit operator Pbkdf2Hash(string value) => new(value);
    public static implicit operator string(Pbkdf2Hash hash) => hash.Value;
    
    /// <summary>
    /// Generates a SHA3-256 hash of the specified password using PBKDF2, or returns the password if it is already hashed.
    /// If the password is not empty and does not start with the prefix, it generates a cryptographically secure random salt of 16 bytes
    /// and uses it along with the password to generate a hash through the PBKDF2 algorithm.
    /// The resulting hash is a Base64 URL safe string.
    /// If the password is empty or already starts with the prefix, this method simply returns the password as is.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The SHA3-256 hash of the password if it was not already hashed, or the original password if it was already hashed.</returns>
    private static string HashPassword(string password)
    {
        if (password.Length > 0) return !password.StartsWith(Prefix) ? Prefix + HashEncode(password) : password;
        return string.Empty;
    }

    /// <summary>
    /// Generates a SHA3-256 hash of the specified password using PBKDF2.
    /// This method creates a cryptographically secure random salt of 16 bytes
    /// and uses it along with the password to generate a hash through the PBKDF2
    /// algorithm with 10,000 iterations. The resulting hash and salt are encoded
    /// in Base64 URL format and concatenated with a dot separator.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A string containing the Base64 URL encoded salt and hash, separated by a dot.</returns>
    private static string HashEncode(string password)
    {
        var salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        var hash = new byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, hash, Iterations, HashAlgorithmName.SHA3_256);
        return Base64UrlEncode(salt) + "." + Base64UrlEncode(hash);
    }
    
    /// <summary>
    /// Encodes a byte array into a Base64 URL safe string.
    /// This method takes a byte array and encodes it into a Base64 URL safe string.
    /// The resulting string will not contain any of the characters '/', '+', or '='.
    /// </summary>
    /// <param name="input">The byte array to encode.</param>
    /// <returns>The Base64 URL safe string representation of the input.</returns>
    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    /// <summary>
    /// Decodes a Base64 URL safe string into a byte array.
    /// This method takes a Base64 URL safe string and decodes it into a byte array.
    /// The resulting byte array is the original byte array that was encoded.
    /// </summary>
    /// <param name="input">The Base64 URL safe string to decode.</param>
    /// <returns>The decoded byte array.</returns>
    private static byte[] Base64UrlDecode(string input)
    {
        var output = input.Replace('-', '+').Replace('_', '/').TrimEnd();
        output += new string('=', (4 - input.Length % 4) % 4);
        return Convert.FromBase64String(output);
    }


    /// <summary>
    /// Compares the given password to the hashed password stored in the instance.
    /// This method takes a password and compares it to the hashed password stored in the instance.
    /// If the password matches the hashed password, the method returns <c>true</c>.
    /// Otherwise, the method returns <c>false</c>.
    /// The method first checks if the stored value starts with the prefix, and if not, returns <c>false</c>.
    /// Then, it splits the stored value into two parts, the salt and the hash.
    /// If the split does not result in two parts, the method returns <c>false</c>.
    /// Next, the method decodes the salt and the hash from the Base64 URL safe strings.
    /// If the decoded salt or hash does not have the expected length, the method returns <c>false</c>.
    /// Finally, the method hashes the given password using the same parameters as the stored hash,
    /// and then compares the two hashed values using <see cref="CryptographicOperations.FixedTimeEquals"/>.
    /// If the two hashed values are equal, the method returns <c>true</c>;
    /// otherwise, the method returns <c>false</c>.
    /// </summary>
    /// <param name="password">The password to compare to the stored hashed password.</param>
    /// <returns><c>true</c> if the given password matches the stored hashed password, <c>false</c> otherwise.</returns>
    public bool ComparePassword(string password)
    {
        if (!Value.StartsWith(Prefix)) return false;

        var parts = Value[Prefix.Length..].Split(".");
        if (parts.Length != 2) return false;

        var salt = Base64UrlDecode(parts[0]);
        var stored = Base64UrlDecode(parts[1]);
        
        if (salt.Length != SaltSize) return false;
        if (stored.Length != HashSize) return false;
        
        var hash = new byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, hash, Iterations, HashAlgorithmName.SHA3_256);
        return CryptographicOperations.FixedTimeEquals(hash, stored);
    }

    /// <summary>
    /// Compares the given hashed password to the hashed password stored in the instance.
    /// This method checks if the provided hashed password matches the hashed password
    /// stored in the current instance of <see cref="Pbkdf2Hash"/>.
    /// </summary>
    /// <param name="hashed">The hashed password to compare.</param>
    /// <returns><c>true</c> if the given hashed password matches the stored hashed password; <c>false</c> otherwise.</returns>
    public bool CompareHashed(string hashed)
    {
        return Value == hashed;
    }

    /// <summary>
    /// Checks if the two <see cref="Pbkdf2Hash"/> instances have the same value.
    /// </summary>
    /// <param name="self">The first <see cref="Pbkdf2Hash"/> instance.</param>
    /// <param name="other">The second <see cref="Pbkdf2Hash"/> instance.</param>
    /// <returns><c>true</c> if the two instances have the same value; <c>false</c> otherwise.</returns>
    public static bool operator ==(Pbkdf2Hash self, Pbkdf2Hash other)
    {
        return self.Value == other.Value;
    }

    /// <summary>
    /// Checks if the two <see cref="Pbkdf2Hash"/> instances do not have the same value.
    /// </summary>
    /// <param name="self">The first <see cref="Pbkdf2Hash"/> instance.</param>
    /// <param name="other">The second <see cref="Pbkdf2Hash"/> instance.</param>
    /// <returns><c>true</c> if the two instances do not have the same value; <c>false</c> otherwise.</returns>
    public static bool operator !=(Pbkdf2Hash self, Pbkdf2Hash other)
    {
        return !(self == other);
    }

    /// <summary>
    /// Checks if the given <see cref="Pbkdf2Hash"/> instance has the same value as the given string.
    /// If the given string starts with the prefix, the method checks if the two strings are equal.
    /// Otherwise, the method calls <see cref="ComparePassword"/> with the given string.
    /// </summary>
    /// <param name="self">The <see cref="Pbkdf2Hash"/> instance to compare.</param>
    /// <param name="other">The string to compare to the value of the <see cref="Pbkdf2Hash"/> instance.</param>
    /// <returns><c>true</c> if the two strings are equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(Pbkdf2Hash self, string other)
    {
        if (other.StartsWith(Prefix)) return self.Value == other;
        return self.ComparePassword(other);
    }

    /// <summary>
    /// Checks if the given <see cref="Pbkdf2Hash"/> instance does not have the same value as the given string.
    /// This method compares the <see cref="Pbkdf2Hash"/> instance to a string. If the string starts with the prefix,
    /// it checks if the two strings are not equal. Otherwise, it calls <see cref="ComparePassword"/> with the given string.
    /// </summary>
    /// <param name="self">The <see cref="Pbkdf2Hash"/> instance to compare.</param>
    /// <param name="other">The string to compare to the value of the <see cref="Pbkdf2Hash"/> instance.</param>
    /// <returns><c>true</c> if the two strings are not equal; <c>false</c> otherwise.</returns>
    public static bool operator !=(Pbkdf2Hash self, string other)
    {
        return !(self == other);
    }

    /// <summary>
    /// Checks if the given object is equal to the current instance of <see cref="Pbkdf2Hash"/>.
    /// This method checks if the given object is an instance of <see cref="Pbkdf2Hash"/> and if its value is equal to the value of the current instance.
    /// </summary>
    /// <param name="obj">The object to compare to the current instance of <see cref="Pbkdf2Hash"/>.</param>
    /// <returns><c>true</c> if the given object is an instance of <see cref="Pbkdf2Hash"/> and its value is equal to the value of the current instance; <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Pbkdf2Hash hash && Value == hash.Value;
    }
    
    /// <summary>
    /// Converts the current instance of <see cref="Pbkdf2Hash"/> to its string representation.
    /// The string representation is the value of the current instance.
    /// </summary>
    /// <returns>The string representation of the current instance of <see cref="Pbkdf2Hash"/>.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Gets the hash code of the current instance of <see cref="Pbkdf2Hash"/>.
    /// The hash code is generated from the value of the current instance.
    /// </summary>
    /// <returns>The hash code of the current instance of <see cref="Pbkdf2Hash"/>.</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}