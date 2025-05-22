using Microsoft.AspNetCore.Builder;

namespace NokoCore.PyLike3;

public class PyLike3WebApplication
{
    // TODO: Wrapper WebApplication.CreateBuilder -> WebAppPyLike3 (Look like WSGI or ASGI)
}

public static partial class PyLike3
{
    // TODO: Wrapper
    public static PyLike3WebApplication CreateWebApplication() => new PyLike3WebApplication();
}

/// <summary>
/// Represents a set of configuration options for the WebAppPyLike3 module,
/// allowing customization of behavior such as verbosity, platform information display,
/// interpreter path, and additional arguments.
/// </summary>
public sealed class PyLike3WebApplicationOptions
{
    public bool Quiet { get; set; }
    public bool PrintPlatformInfo { get; set; } = true;
    public string? InterpreterPath { get; set; }
    public string[] Args { get; set; } = [];
}

/// <summary>
/// Represents a delegate used to configure options for the WebAppPyLike3 module.
/// </summary>
/// <param name="options">The <see cref="PyLike3WebApplicationOptions"/> to be configured.</param>
public delegate void PyLike3WebApplicationConfigureDelegate(PyLike3WebApplicationOptions options);

/// <summary>
/// Provides extension methods for configuring and initializing the PyLike3 utility module
/// within a web application framework.
/// </summary>
public static class PyLike3WebApplicationExtensions
{
    /// <summary>
    /// Configures the <see cref="PyLike3"/> module options and initializes the module.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <param name="configure">A delegate that configures the <see cref="PyLike3WebApplicationOptions"/>.</param>
    /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UsePyLike3(this IApplicationBuilder app, PyLike3WebApplicationConfigureDelegate configure)
    {
        var options = new PyLike3WebApplicationOptions();
        configure.Invoke(options);
        
        var args = options.Args.ToList();
        if (options.Quiet) args.Add("--quiet");
        if (!options.PrintPlatformInfo) args.Add("--no-platform-info");
        if (!string.IsNullOrEmpty(options.InterpreterPath)) args.AddRange(["--interpreter-path", options.InterpreterPath]);

        PyLike3.Init(args.ToArray());
        return app;
    }
}