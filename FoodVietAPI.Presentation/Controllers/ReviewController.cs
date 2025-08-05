using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

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
        [SwaggerOperation (Summary = "Create a review for an order detail")]
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
                return BadRequest(ModelState);

            // inject route values into the DTO
            req.RetailerId = retailerId;
            req.OrderDetailId = orderDetailId;

            try
            {
                var dto = await _reviewSvc.CreateReviewAsync(req);
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
        public async Task<IActionResult> GetReviewForOrderDetail(
            [FromRoute] string retailerId,
            [FromRoute] string orderId,
            [FromRoute] string orderDetailId)
        {
            var dto = await _reviewSvc.GetReviewForOrderDetailAsync(retailerId, orderDetailId);
            if (dto == null) return NoContent();
            return Ok(dto);
        }

        // GET /api/v1/products/{productId}/reviews
        //[HttpGet(ApiEndpointConstant.Product.ProductReviewsEndpoint)]
        //[SwaggerOperation(Summary = "Get all reviews for a product (supposed to put this in post)")]
        //[ProducesResponseType(typeof(IReadOnlyList<ProductReviewDTO>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetReviewsForProduct([FromRoute] string productId)
        //{
        //    var list = await _reviewSvc.GetProductReviewsAsync(productId);
        //    return Ok(list);
        //}
    }
}
