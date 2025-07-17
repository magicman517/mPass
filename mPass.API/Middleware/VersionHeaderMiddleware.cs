using System.Reflection;

namespace mPass.API.Middleware;

public class VersionHeaderMiddleware(RequestDelegate next)
{
    private static string ApiVersion => Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split("+")[0] ?? "unknown";

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers["x-mpass-version"] = ApiVersion;
        await next(context);
    }
}

public static class VersionHeaderMiddlewareExtensions
{
    public static IApplicationBuilder UseVersionHeader(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<VersionHeaderMiddleware>();
    }
}