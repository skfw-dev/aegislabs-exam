using NokoCore.Common;

namespace NokoCore.PyLike3;

/// <summary>
/// Provides functionality for parsing command-line arguments and retrieving associated values.
/// </summary>
public class ArgsParser
{
    /// <summary>
    /// Tracks the current update state of command-line arguments in the <see cref="ArgsParser"/> class.
    /// </summary>
    /// <remarks>
    /// This variable is incremented whenever the arguments are updated, allowing the class to manage concurrency when parsing and retrieving values.
    /// </remarks>
    private int _updateIndex;

    private int _currentIndex;
    
    /// <summary>
    /// A lock object used to synchronize access to the <see cref="ArgsParser"/> class.
    /// </summary>
    private readonly Lock _lock = new();

    /// <summary>
    /// Stores the list of command-line arguments provided to the parser.
    /// </summary>
    /// <remarks>
    /// This property contains the original arguments passed to the <see cref="ArgsParser"/>.
    /// It is updated whenever new arguments are parsed through the <see cref="ArgsParser.Parse"/> method.
    /// </remarks>
    public List<string> Args { get; private set; }
    public Dictionary<string, string?> Kwargs { get; private set; } = new();
    public List<string> Values { get; private set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgsParser"/> class with the specified command-line arguments.
    /// </summary>
    /// <param name="args">An array of command-line arguments to parse.</param>
    public ArgsParser(params string[] args)
    {
        Args = args.ToList();
        _updateIndex = 1;
    }

    /// <summary>
    /// Parses the command-line arguments.
    /// </summary>
    /// <param name="args">An array of command-line arguments to parse.</param>
    /// <returns>A dictionary where the keys are the names of the command-line arguments, and the values are the values of the command-line arguments.</returns>
    /// <remarks>
    /// If the <paramref name="args"/> array is empty, the method simply returns the current dictionary of command-line arguments.
    /// </remarks>
    public ArgsParser Parse(params string[] args)
    {
        lock (_lock)
        {
            if (args.Length > 0)
            {
                Args = args.ToList();
                _updateIndex += 1;
            }

            if (_currentIndex == _updateIndex)
            {
                return this;
            }

            var lastKey = "";
            var flags = new[] { "--", "-", "/" };

            // Reset values
            Kwargs = new Dictionary<string, string?>();
            Values = [];
            
            foreach (var arg in Args)
            {
                var flag = flags.FirstOrDefault(c => arg.StartsWith(c));
                if (flag != null)
                {
                    var key = arg[flag.Length..];
                    Kwargs[key] = null;
                    lastKey = key;
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastKey))
                    {
                        Kwargs[lastKey] = arg;
                    }
                    else
                    {
                        // Maybe this is output a file or something.
                        Values.Add(arg);
                    }
                }
            }

            _currentIndex += 1;
            return this;
        }
    }
    
    /// <summary>
    /// Finds a key in the dictionary of command-line arguments.
    /// </summary>
    /// <param name="keys">A list of keys to search for.</param>
    /// <returns>A tuple containing a boolean indicating whether the key was found and the value of the key if it was found.</returns>
    public IResult<string> Find(params string[] keys)
    {
        var flags = new[] { "--", "-", "/" };

        foreach (var key in keys)
        {
            // Trim flag prefixes dynamically
            var cleanKey = flags.Aggregate(key, (current, flag) =>
                current.StartsWith(flag) ? current[flag.Length..] : current);

            // Search in Kwargs after cleaning the key
            if (Kwargs.TryGetValue(cleanKey, out var value)) return Result<string>.Set(value);
        }

        return Result<string>.Set();
    }
}

/// <summary>
/// Utility class that mimics Python's built-in functions.
/// </summary>
public static partial class PyLike3
{
    /// <summary>
    /// Parses the command-line arguments and returns an <see cref="ArgsParser"/>.
    /// </summary>
    /// <param name="args">The command-line arguments to parse.</param>
    /// <returns>An <see cref="ArgsParser"/> that contains the parsed arguments.</returns>
    public static ArgsParser ArgsParser(params string[] args) => new(args);
}
