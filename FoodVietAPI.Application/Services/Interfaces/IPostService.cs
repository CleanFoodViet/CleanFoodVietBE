using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<IPaginate<PostListDTO>> GetPostList(int page, int size);
        Task<PostDTO> GetPostInformation(string postId);

        Task UpdatePostStatus(string postId, string status);
        Task<List<PostListDTO>> GetRetailerFavoritePost(string retailerId);
        Task AddPostToFavorite(string postId, string retailerId);
        Task RemovePostFromFavorite(string postId, string retailerId);
    }
}
