using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class FavoriteController : BaseController<FavoriteController>
    {
        private readonly IPostService _postService;
        public FavoriteController(ILogger<FavoriteController> logger, IPostService postService) : base(logger)
        {
            _postService = postService;
        }

        //Retailer's Favorite post APIs controller
        [HttpGet(ApiEndpointConstant.Favorite.RetailerFavPostEndpoint)]
        [ProducesResponseType(typeof(List<PostListDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoritePostList([FromRoute] string retailerId)
        {
            var res = await _postService.GetRetailerFavoritePost(retailerId);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Favorite.RetailerFavPostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPostToFavorite([FromRoute] string id, [FromRoute] string postId)
        {
            await _postService.AddPostToFavorite(postId, id);
            return Ok("Add post to favorite successfully");
        }

        [HttpDelete(ApiEndpointConstant.Favorite.RetailerFavPostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePostFromFavorite([FromRoute] string id, [FromRoute] string postId)
        {
            await _postService.RemovePostFromFavorite(postId, id);
            return Ok("Remove Post from favorite successfully");
        }
    }
}
