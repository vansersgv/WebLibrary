using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string BasicScheme = "Basic";

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith(BasicScheme))
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Substring(BasicScheme.Length).Trim())).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                
                var validUsername = Environment.GetEnvironmentVariable("AUTH_USER");
                var validPassword = Environment.GetEnvironmentVariable("AUTH_PASSWORD");
               
                if (username == validUsername && password == validPassword)
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.Headers["WWW-Authenticate"] = BasicScheme;
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}
