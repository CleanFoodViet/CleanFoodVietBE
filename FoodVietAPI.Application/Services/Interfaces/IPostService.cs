using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<IPaginate<PostListDTO>> GetPostList(int page, int size);
        Task<PostDTO> GetPostInformation(string postId);
    }
}
