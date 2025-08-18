using System.Net;
using System.Text.Json;
using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Data.Exceptions;

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
            var response = context.Response;
            response.ContentType = "application/json";

            var errorDto = new ErrorDTO
            {
                TimeStamp = DateTime.UtcNow,
                Error = exception.Message
            };

            switch (exception)
            {
                case BadHttpRequestException badReq:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDto.StatusCode = response.StatusCode;
                    break;

                case DomainValidationException domEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDto.StatusCode = response.StatusCode;
                    break;

                case NotFoundException notFound:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorDto.StatusCode = response.StatusCode;
                    break;

                case UnauthorizedAccessException _:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorDto.StatusCode = response.StatusCode;
                    break;

                case DeletionRestrictedException _:
                case UpdateRestrictedException _:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorDto.StatusCode = response.StatusCode;
                    break;

                case ForbiddenException _:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorDto.StatusCode = response.StatusCode;
                    break;

                default:
                    // Unhandled exception
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorDto.StatusCode = response.StatusCode;
                    errorDto.Error = exception.Message;
                    _logger.LogError(exception, "Unhandled exception");
                    break;
            }

            var json = JsonSerializer.Serialize(errorDto);
            await response.WriteAsync(json);
        }
    }
}
