using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.DTOs.PostMediaDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Enums.PostEnums;
using CleanFoodVietAPI.Data.Enums.PostMedia;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class PostService : BaseService<PostService>, IPostService
    {
        public PostService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<PostService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<PostListDTO>> GetPostList(int page, int size)
        {
            var postList = await _unitOfWork.GetRepository<Post>()
                .GetPagingListAsync(
                include: po => po.Include(x => x.PostMedias.Where(pm => pm.MediumType.Equals(PostMediaTypeEnum.THUMBNAIL.ToString())))
                                 .Include(x => x.Account).Include(x => x.Product).ThenInclude(x => x.ProductPrices.Where(pp => pp.IsCurrent)),
                predicate: po => po.Status.Equals(PostStatusEnum.ACTIVE.ToString()),
                selector: po => new PostListDTO(
                    po.PostId,
                    po.Title,
                    po.Product.ProductPrices.First().Price,
                    po.Product.ProductPrices.First().Currency,
                    po.PostMedias.First().MediumUrl,
                    po.Account.Name,
                    po.Account.Avatar),
                page: page, size: size);

            return postList;
        }

        public async Task<PostDTO> GetPostInformation(string postId)
        {
            Ulid postID = Ulid.Parse(postId);
            var post = await _unitOfWork.GetRepository<Post>()
                .GetAsync(
                    include: po => po.Include(x => x.PostMedias).Include(x => x.Account).Include(x => x.Product),
                    predicate: po => po.PostId == postID,
                    selector: po => new PostDTO
                    {
                        PostId = po.PostId,
                        Title = po.Title,
                        Content = po.Content,
                        HarvestDate = po.HarvestDate,
                        PostStatus = po.Status,
                        Rating = po.Rating,
                        CreatedAt = po.CreatedAt,
                        PostEndDate = po.PostEndDate,
                        Priority = po.Priority,
                        GardenerId = po.GardenerId,
                        GardenderName = po.Account.Name,
                        GardenerAvatar = po.Account.Avatar,
                        ProductId = po.ProductId
                    }
                );

            if (post == null) throw new BadHttpRequestException("Post cannot be found");

            var postProduct = await _unitOfWork.GetRepository<Product>()
                .GetAsync(
                    include: p => p.Include(x => x.ProductPrices.Where(pp => pp.IsCurrent)).Include(x => x.ProductCategory),
                    predicate: p => p.ProductId == post.ProductId,
                    selector: p => new PostProductDTO(
                        p.ProductName,
                        p.UpdatedAt,
                        p.Status,
                        p.ProductCategory.Name,
                        p.ProductPrices.First().Price,
                        p.ProductPrices.First().Currency)
                );

            var postMedias = await _unitOfWork.GetRepository<PostMedia>()
                .GetListAsync(predicate: pm => pm.PostId == postID,
                              selector: pm => new PostMediaDTO
                              (
                                  pm.PostMediaId,
                                  pm.MediumUrl,
                                  pm.MediumType,
                                  pm.UploadedAt
                              ));

            post.PostMedias = postMedias.ToList();
            post.ProductData = postProduct;

            return post;
        }

        public async Task UpdatePostStatus(string postId, string status)
        {
            Ulid postID = Ulid.Parse(postId);
            var post = await _unitOfWork.GetRepository<Post>()
                .GetAsync(predicate: p => p.PostId == postID);

            if (post == null) throw new BadHttpRequestException("Post cannot be found");

            if (Enum.TryParse<PostStatusEnum>(status.ToUpper(), out var result))
            {
                post.Status = result.ToString();
            }
            else
            {
                post.Status = PostStatusEnum.INACTIVE.ToString();
            }

            _unitOfWork.GetRepository<Post>().UpdateAsync(post);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when updating post status");
        }
    }
}
