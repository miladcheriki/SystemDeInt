using Microsoft.AspNetCore.Http;
using Serilog;

namespace AvesdoSystemDesign.Shared.Logger;

public class HttpLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        
        var requestLog = new
        {
            Method = request.Method,
            Path = request.Path,
            QueryString = request.QueryString.ToString(),
            Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
        };

        Log.Information("Request: {@RequestLog}", requestLog);
        
        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await next(context);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var responseLog = new
        {
            StatusCode = context.Response.StatusCode,
            Body = responseText
        };

        Log.Information("Response: {@ResponseLog}", responseLog);
        
        await responseBodyStream.CopyToAsync(originalResponseBodyStream);
    }
}