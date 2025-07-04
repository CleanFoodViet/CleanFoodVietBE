using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<IPaginate<PostListDTO>> GetPostList(
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? search = null);
        Task<IPaginate<PostListDTO>> GetGardenerPostList(
            string gardenerId,
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? search = null);
        Task<PostDTO> GetPostInformation(string postId);
        Task CreatePost(string gardenerId, CreatePostDTO request);
        Task UpdatePost(string postId, UpdatePostDTO request);
        Task UpdatePostStatus(string postId, string status);
        Task<List<PostListDTO>> GetRetailerFavoritePost(string retailerId);
        Task AddPostToFavorite(string postId, string retailerId);
        Task RemovePostFromFavorite(string postId, string retailerId);
    }
}
