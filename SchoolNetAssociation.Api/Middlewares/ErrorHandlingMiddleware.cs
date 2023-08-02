using System.Net;
using System.Text.Json;

namespace SchoolNetAssociation.Api.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_logger = logger;
			_next = next;
		}
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error occured in {httpContext.Request.Method} {httpContext.Request.Path}: {ex}");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			string message = ex.Message;
			int statusCode;

			switch (ex)
			{
				case InvalidOperationException:
					statusCode = (int)HttpStatusCode.BadRequest;
					break;
				case KeyNotFoundException:
					statusCode = (int)HttpStatusCode.NotFound;
					break;
				case UnauthorizedAccessException:
					statusCode = (int)HttpStatusCode.Unauthorized;
					break;
				default:
					statusCode = (int)HttpStatusCode.InternalServerError;
					message = "Internal Server Error.";
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;
			var response = new { StatusCode = statusCode, Message = message };
			var result = JsonSerializer.Serialize(response);
			return context.Response.WriteAsync(result);
		}
	}
}
