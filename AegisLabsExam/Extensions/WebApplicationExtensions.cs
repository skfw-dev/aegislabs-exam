using Rotativa.AspNetCore;

namespace AegisLabsExam.Extensions;

public sealed class RotativaOptions
{
    public string RootPath { get; set; } = null!;
}

public delegate void RotativaConfigureDelegate(RotativaOptions options); 

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseRotativa(this IApplicationBuilder app, RotativaConfigureDelegate configure)
    {
        var options = new RotativaOptions();
        configure.Invoke(options);
        RotativaConfiguration.Setup(options.RootPath);
        return app;
    }
}