using System.Diagnostics;
using System.Net;

namespace Library.Web.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, IWebHostEnvironment environment)
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

                // Handle 404 and other non-success status codes
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleNotFoundException(context);
                }
                else if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500)
                {
                    await HandleClientError(context);
                }
                else if (context.Response.StatusCode >= 500)
                {
                    await HandleServerError(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request processing.");
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            // Log the exception with full details
            _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);

            // Prevent response from being sent if it has already started
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response has already started; cannot send error response.");
                return;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/html";

            // Store exception details in HttpContext for the error page
            context.Items["Exception"] = exception;
            context.Items["ExceptionMessage"] = exception.Message;
            context.Items["ExceptionStackTrace"] = _environment.IsDevelopment() ? exception.StackTrace : null;
            context.Items["RequestId"] = context.TraceIdentifier;

            // Redirect to error page
            context.Request.Path = "/Home/Error";
            
            var routeData = context.GetRouteData();
            if (routeData == null)
            {
                routeData = new Microsoft.AspNetCore.Routing.RouteData();
                context.Items["RouteData"] = routeData;
            }

            routeData.Values["controller"] = "Home";
            routeData.Values["action"] = "Error";

            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                context.SetEndpoint(new Microsoft.AspNetCore.Http.Endpoint(null, new Microsoft.AspNetCore.Http.EndpointMetadataCollection(), "Error"));
            }

            // Call the error action
            await _next(context);
        }

        private async Task HandleNotFoundException(HttpContext context)
        {
            _logger.LogInformation("404 Not Found - Path: {Path}", context.Request.Path);

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/html";
                context.Items["StatusCode"] = 404;
                context.Items["StatusMessage"] = "Page Not Found";
                context.Items["RequestId"] = context.TraceIdentifier;

                context.Request.Path = "/Home/Error";
                await _next(context);
            }
        }

        private async Task HandleClientError(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                var statusCode = context.Response.StatusCode;
                _logger.LogInformation("Client Error {StatusCode} - Path: {Path}", statusCode, context.Request.Path);

                context.Items["StatusCode"] = statusCode;
                context.Items["StatusMessage"] = GetStatusMessage(statusCode);
                context.Items["RequestId"] = context.TraceIdentifier;

                context.Request.Path = "/Home/Error";
                await _next(context);
            }
        }

        private async Task HandleServerError(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                var statusCode = context.Response.StatusCode;
                _logger.LogError("Server Error {StatusCode} - Path: {Path}", statusCode, context.Request.Path);

                context.Response.StatusCode = statusCode;
                context.Items["StatusCode"] = statusCode;
                context.Items["StatusMessage"] = GetStatusMessage(statusCode);
                context.Items["RequestId"] = context.TraceIdentifier;

                context.Request.Path = "/Home/Error";
                await _next(context);
            }
        }

        private static string GetStatusMessage(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            405 => "Method Not Allowed",
            408 => "Request Timeout",
            409 => "Conflict",
            410 => "Gone",
            413 => "Payload Too Large",
            414 => "URI Too Long",
            415 => "Unsupported Media Type",
            429 => "Too Many Requests",
            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            504 => "Gateway Timeout",
            _ => "An Error Occurred"
        };
    }
}
