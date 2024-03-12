using API.Errors;
using System.Text.Json;


namespace API.Middleware;

public class ExceptionMiddleware
{
    private const int INTERNAL_SERVER_ERROR = 500;

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment environment
        )
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = INTERNAL_SERVER_ERROR;

            var response = _environment.IsDevelopment()
                ? new ApiException(INTERNAL_SERVER_ERROR, ex.Message, ex.StackTrace.ToString())
                : new ApiException(INTERNAL_SERVER_ERROR);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }

    }
}
