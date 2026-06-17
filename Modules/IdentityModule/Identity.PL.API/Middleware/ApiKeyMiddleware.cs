using Identity.PL.API.Common;
using Identity.PL.API.Common.Validator;
using System.ComponentModel.DataAnnotations;

namespace Identity.PL.API.Middleware;

    // C# - ApiKeyMiddleware (resolve per request)
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYHEADER = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<ApiKeyAttribute>() is null)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(APIKEYHEADER, out var apiKey))
        {
            context.Response.StatusCode = 401;
            return;
        }

        var validator = context.RequestServices.GetRequiredService<IApiKeyValidator>();
        if (!validator.IsValid(apiKey!))
        {
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
     }
}

