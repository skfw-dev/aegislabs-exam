using System.Text.RegularExpressions;

namespace AegisLabsExam.Common;

public static partial class TransformText
{
    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex UpperSnippetRegex();

    [GeneratedRegex(@"[\s_-]+")]
    private static partial Regex RemoveSpacesRegex();
    
    private static string Capitalize(string input) => input.Length switch
    {
        0 => string.Empty,
        1 => input.ToUpper(),
        _ => char.ToUpper(input[0]) + input[1..].ToLower()
    };
    
    /// <summary>
    /// Converts the input string to a Pascal case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any camel case or snake case strings to a Pascal case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The Pascal case version of the input string.</returns>
    public static string ToPascalCase(string input)
    {
        var words = RemoveSpacesRegex().Split(input.Trim());
        return string.Join("", words.Select(Capitalize));
    }

    /// <summary>
    /// Converts the input string to a camel case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any PascalCase strings to a camel case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The camel case version of the input string.</returns>
    public static string ToCamelCase(string input)
    {
        var output = ToPascalCase(input);
        return output.Length switch
        {
            0 => string.Empty,
            1 => output.ToLower(),
            _ => char.ToLower(output[0]) + output[1..]
        };
    }
    
    /// <summary>
    /// Converts the input string to a title case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any camel case, PascalCase, or snake case strings to a title case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The title case version of the input string.</returns>
    public static string ToTitleCase(string input)
    {
        var words = RemoveSpacesRegex().Split(input.Trim());
        return string.Join(" ", words.Select(Capitalize));
    }

    /// <summary>
    /// Converts the input string to an upper snake case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any camel case or PascalCase strings to an upper snake case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The upper snake case version of the input string.</returns>
    public static string ToUpperSnakeCase(string input)
    {
        var output = UpperSnippetRegex().Replace(input.Trim(), "_$1");
        output = RemoveSpacesRegex().Replace(output, "_");
        return output.ToUpper();
    }
    
    /// <summary>
    /// Converts the input string to a snake case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any camel case or PascalCase strings to a snake case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The snake case version of the input string.</returns>
    public static string ToSnakeCase(string input)
    {
        var output = UpperSnippetRegex().Replace(input.Trim(), "_$1");
        output = RemoveSpacesRegex().Replace(output, "_");
        return output.ToLower();
    }
    
    /// <summary>
    /// Converts the input string to an upper kebab case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any camel case or PascalCase strings to an upper kebab case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The upper kebab case version of the input string.</returns>
    public static string ToUpperKebabCase(string input)
    {
        var output = UpperSnippetRegex().Replace(input.Trim(), "-$1");
        output = RemoveSpacesRegex().Replace(output, "-");
        return output.ToUpper();
    }
    
    /// <summary>
    /// Converts the input string to a kebab case string.
    /// This method trims the input string, removes any spaces, hyphens, or underscores,
    /// and then converts any camel case or PascalCase strings to a kebab case string.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The kebab case version of the input string.</returns>
    public static string ToKebabCase(string input)
    {
        var output = UpperSnippetRegex().Replace(input.Trim(), "-$1");
        output = RemoveSpacesRegex().Replace(output, "-");
        return output.ToLower();
    }
}