using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class PostController : BaseController<PostController>
    {
        private readonly IPostService _postService;
        public PostController(ILogger<PostController> logger, IPostService postService) : base(logger)
        {
            _postService = postService;
        }

        [HttpGet(ApiEndpointConstant.Post.PostsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<PostListDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all available Post List")]
        public async Task<IActionResult> GetPostList(
            [FromQuery]int page = 1, [FromQuery]int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? search = null,
            [FromQuery] string? address = null)
        {
            var res = await _postService.GetPostList(page, size, filterField, filterValue, search, address);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Post.GardenerPostEndpoint)]
        [ProducesResponseType(typeof(IPaginate<PostListDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all Gardener Post List")]
        public async Task<IActionResult> GetGardenerPostList(
            [FromRoute] string id,
            [FromQuery] int page = 1, [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? search = null)
        {
            var res = await _postService.GetGardenerPostList(id, page, size, filterField, filterValue, search);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Post.PostEndpoint)]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Post detail")]
        public async Task<IActionResult> GetPostInformation([FromRoute]string id)
        {
            var res = await _postService.GetPostInformation(id);
            return Ok(res);
        }

        //Create Post
        [HttpPost(ApiEndpointConstant.Post.GardenerPostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create Post")]
        public async Task<IActionResult> CreatePost([FromRoute]string id, [FromBody]CreatePostDTO request)
        {
            await _postService.CreatePost(id, request);
            return StatusCode(StatusCodes.Status201Created, "Create post successsfully");
        }

        //Update Post (basic information)
        [HttpPatch(ApiEndpointConstant.Post.PostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update Post detail (Title, content)")]
        public async Task<IActionResult> UpdatePost([FromRoute] string id, [FromBody]UpdatePostDTO request)
        {
            await _postService.UpdatePost(id, request);
            return Ok("Update post successfully");
        }

        //Update Status
        [HttpPatch(ApiEndpointConstant.Post.PostStatusEndpoint)]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update Post Status (Post Status: ACTIVE, INACTIVE, BANNED)")]
        public async Task<IActionResult> UpdatePostStatus([FromRoute] string id, [FromQuery]string status)
        {
            await _postService.UpdatePostStatus(id, status);
            return Ok($"Update post status to {status.ToUpper()} successfully");
        }
    }
}
