using System.Runtime.InteropServices;

namespace NokoCore.PyLike3;

/// <summary>
/// Interface that mimics Python's type.
/// </summary>
public interface IPyLike3;

/// <summary>
/// Interface that mimics Python's default type.
/// </summary>
public interface IPyLike : IPyLike3;

/// <summary>
/// Utility class that mimics Python's built-in functions.
/// </summary>
public static partial class PyLike3
{
    /// <summary>
    /// The version of PyLike3.
    /// </summary>
    public static Version Version => new(3, 0, 0);
    
    /// <summary>
    /// Returns the platform information for PyLike3.
    /// </summary>
    /// <returns>
    /// A string containing the PyLike3 version, .NET version, module name,
    /// current date and time, .NET compiler version, OS description, and
    /// platform information.
    /// </returns>
    public static string GetPlatformInfo()
    {
        var dotNetVersion = RuntimeInformation.FrameworkDescription;
        var programName = AppDomain.CurrentDomain.FriendlyName;
        var dateTimeFormatted = DateTime.Now.ToString("MMM dd yyyy, HH:mm:ss");
        var platform = RuntimeInformation.OSArchitecture.ToString();
        var osDescription = RuntimeInformation.OSDescription;
        var compilerInfo = $"C# {Environment.Version}";
        return $"PyLike3 {Version} | using SDK {dotNetVersion} | ({programName}, {dateTimeFormatted}) [{compilerInfo}] on {osDescription} ({platform})";
    }
    
    /// <summary>
    /// Prints the platform information of PyLike3 to the console.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Init(params string[] args)
    {
        var argsParser = new ArgsParser();
        argsParser.Parse(args);
        var quietMode = argsParser.Find("--quiet", "-quiet");
        var disablePlatformInfo = argsParser.Find("--no-platform-info");
        if (!quietMode.Ok && !disablePlatformInfo.Ok) Console.WriteLine(GetPlatformInfo());
        var interpreterPath = argsParser.Find("--interpreter-path");
        if (interpreterPath.Ok) Console.WriteLine($"Interpreter Path: {interpreterPath.Unwrap()}");
    }
}
