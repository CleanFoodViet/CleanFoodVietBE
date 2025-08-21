using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewSvc;

        public ReviewController(IReviewService reviewSvc)
            => _reviewSvc = reviewSvc;

        // POST /api/v1/retailer/{retailerId}/orders/{orderId}/details/{orderDetailId}/reviews
        [HttpPost(ApiEndpointConstant.Review.CreateForOrderDetail)]
        [SwaggerOperation(Summary = "Create a review for an order detail")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateReviewForOrderDetail(
            [FromRoute] string retailerId,
            [FromRoute] string orderId,
            [FromRoute] string orderDetailId,
            [FromBody] CreateReviewRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate rating range
            if (req.Rating < 1 || req.Rating > 5)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid rating",
                    Detail = "Rating must be between 1 and 5.",
                    Extensions = { ["errorCode"] = "RatingOutOfRange" }
                });
            }

            try
            {
                var dto = await _reviewSvc.CreateReviewAsync(
                    Ulid.Parse(retailerId),
                    Ulid.Parse(orderId),
                    Ulid.Parse(orderDetailId),
                    req.Rating,
                    req.Comment ?? string.Empty
                );

                return Ok(dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid review request",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "ReviewInvalid" }
                });
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not found",
                    Detail = nf.Message
                });
            }
        }


        // GET /api/v1/retailer/{retailerId}/orders/{orderId}/details/{orderDetailId}/review
        [HttpGet(ApiEndpointConstant.Review.GetForOrderDetail)]
        [SwaggerOperation(Summary = "Get a review for an order detail")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReviewForOrderDetail(
            [FromRoute] string retailerId,
            [FromRoute] string orderId,
            [FromRoute] string orderDetailId)
        {
            // 1) Validate Ulid format
            if (!Ulid.TryParse(retailerId, out _))
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid retailerId",
                    Detail = $"'{retailerId}' is not a valid Ulid.",
                    Extensions = { ["errorCode"] = "InvalidRetailerId" }
                });
            if (!Ulid.TryParse(orderId, out _))
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid orderId",
                    Detail = $"'{orderId}' is not a valid Ulid.",
                    Extensions = { ["errorCode"] = "InvalidOrderId" }
                });
            if (!Ulid.TryParse(orderDetailId, out _))
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid orderDetailId",
                    Detail = $"'{orderDetailId}' is not a valid Ulid.",
                    Extensions = { ["errorCode"] = "InvalidOrderDetailId" }
                });

            try
            {
                var dto = await _reviewSvc.GetReviewForOrderDetailAsync(
                    retailerId, orderDetailId);

                if (dto == null)
                    return NoContent();

                return Ok(dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid request",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "ReviewInvalid" }
                });
            }
        }

        // GET /api/v1/products/{productId}/reviews
        [HttpGet(ApiEndpointConstant.Review.ProductReviewDetail)]
        [SwaggerOperation(Summary = "Get all reviews for a product")]
        [ProducesResponseType(typeof(IReadOnlyList<ProductReviewDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReviewsForProduct(
            [FromRoute] string productId)
        {
            // Validate Ulid format
            if (!Ulid.TryParse(productId, out _))
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid productId",
                    Detail = $"'{productId}' is not a valid Ulid.",
                    Extensions = { ["errorCode"] = "InvalidProductId" }
                });

            try
            {
                var list = await _reviewSvc.GetProductReviewsAsync(productId);
                return Ok(list);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid request",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "ReviewInvalid" }
                });
            }

        }
    }
}
