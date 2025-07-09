using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Data.Exceptions;
using System.Net;

namespace CleanFoodVietAPI.Presentation.Middlewares
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalException> _logger;
        public GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorDTO() { TimeStamp = DateTime.UtcNow, Error = exception.Message }; ;
            switch (exception)
            {
                case BadHttpRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case DeletionRestrictedException:
                case UpdateRestrictedException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case ForbiddenException:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    //Unhandle Error/Exception
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            _logger.LogError($"Error at {DateTime.UtcNow} - {exception}");
            var result = errorResponse.ToString();
            await context.Response.WriteAsync(result);
        }
    }
}
