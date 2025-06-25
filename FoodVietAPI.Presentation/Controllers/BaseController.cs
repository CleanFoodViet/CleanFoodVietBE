using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [Route(ApiEndpointConstant.ApiEndpoint)]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        protected ILogger<T> _logger;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected IActionResult? ValidateFilterAndSort<T>(
        string? filterField,
        string? sortField)
        {
            // FILTER
            if (!string.IsNullOrWhiteSpace(filterField))
            {
                var pi = typeof(T).GetProperty(filterField);
                if (pi == null)
                    return BadRequest(new
                    {
                        error = $"Invalid filter field '{filterField}'."
                    });

                // only allowing string or enum filters right now
                if (pi.PropertyType != typeof(string) && !pi.PropertyType.IsEnum)
                    return BadRequest(new
                    {
                        error = $"Filtering on '{filterField}' of type '{pi.PropertyType.Name}' is not supported."
                    });
            }

            // SORT
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var pi = typeof(T).GetProperty(sortField);
                if (pi == null)
                    return BadRequest(new
                    {
                        error = $"Invalid sort field '{sortField}'."
                    });
            }

            return null; // OK
        }
    }
}
