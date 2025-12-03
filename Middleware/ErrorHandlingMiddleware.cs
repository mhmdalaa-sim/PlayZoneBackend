using System.Net;
using System.Text.Json;
using PlayZone.DTOs;

namespace PlayZone.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
  _next = next;
   _logger = logger;
  }

    public async Task InvokeAsync(HttpContext context)
    {
        try
   {
     await _next(context);
      }
   catch (Exception ex)
     {
   await HandleExceptionAsync(context, ex);
   }
    }

private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var code = HttpStatusCode.InternalServerError;
  var result = string.Empty;

 switch (exception)
   {
   case KeyNotFoundException:
       code = HttpStatusCode.NotFound;
  result = JsonSerializer.Serialize(ApiResponse.ErrorResponse(
            exception.Message
     ));
         break;
  
       case ArgumentException:
         code = HttpStatusCode.BadRequest;
         result = JsonSerializer.Serialize(ApiResponse.ErrorResponse(
   exception.Message
 ));
       break;

       case InvalidOperationException:
    code = HttpStatusCode.Conflict;
   result = JsonSerializer.Serialize(ApiResponse.ErrorResponse(
       exception.Message
      ));
     break;
      
       case UnauthorizedAccessException:
      code = HttpStatusCode.Unauthorized;
 result = JsonSerializer.Serialize(ApiResponse.ErrorResponse(
     "Unauthorized access"
 ));
     break;
    
   default:
result = JsonSerializer.Serialize(ApiResponse.ErrorResponse(
      "An internal server error occurred",
  new { message = exception.Message }
        ));
          break;
}

   context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
  }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
