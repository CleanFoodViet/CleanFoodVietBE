using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetPostList([FromQuery]int page = 1, [FromQuery]int size = 10)
        {
            var res = await _postService.GetPostList(page, size);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Post.PostEndpoint)]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostInformation([FromRoute]string id)
        {
            var res = await _postService.GetPostInformation(id);
            return Ok(res);
        }
    }
}
