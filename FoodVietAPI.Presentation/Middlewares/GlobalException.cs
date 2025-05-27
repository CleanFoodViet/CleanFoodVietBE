using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Presentation.Logs.LogConfigs;
using System.Net;

namespace CleanFoodVietAPI.Presentation.Middlewares
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        public GlobalException(RequestDelegate next)
        {
            _next = next;
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
            int statusCode = 500;

            switch (exception)
            {
                case BadHttpRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    //Unhandle Error/Exception
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var errorResponse = new ErrorDTO(statusCode, exception.Message, DateTime.UtcNow);
            var result = errorResponse.ToString();
            LogException.LogExceptions(exception);
            await context.Response.WriteAsync(result);
        }
    }
}
