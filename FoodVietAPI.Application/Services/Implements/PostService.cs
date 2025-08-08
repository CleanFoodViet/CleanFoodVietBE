using AutoMapper;
using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.DTOs.PostMediaDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Enums.PostEnums;
using CleanFoodVietAPI.Data.Enums.PostMedia;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Security.Principal;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class PostService : BaseService<PostService>, IPostService
    {
        public PostService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<PostService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<PostListDTO>> GetPostList(
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? search = null,
            string? address = null,
            double? range = null)
        {
            var specification = new PostSpecification(filterField, filterValue, "Priority", "asc", search);

            List<Ulid>? accountIds = await GetGardnerIdHaveLocationInRange(address, range);

            var postList = await _unitOfWork.GetRepository<Post>()
                .GetPagingListAsync(
                include: po => po.Include(x => x.PostMedias.Where(pm => pm.MediumType.Equals(PostMediaTypeEnum.THUMBNAIL.ToString())))
                                 .Include(x => x.Account).ThenInclude(x => x.Addresses)
                                 .Include(x => x.Product).ThenInclude(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                 .Include(x => x.Product.ProductCertificates),
                predicate: po => po.Status.Equals(PostStatusEnum.ACTIVE.ToString()) && 
                                 (accountIds != null ? accountIds.Any(x => x == po.GardenerId) : true),
                spec: specification,
                selector: po => new PostListDTO(
                    po.PostId,
                    po.Title,
                    po.Product.ProductPrices.First().Price,
                    po.Product.ProductPrices.First().Currency,
                    po.PostMedias.First().MediumUrl,
                    po.Account.Name,
                    po.Account.Avatar,
                    po.CreatedAt,
                    po.Content,
                    po.Status,
                    po.Rating,
                    po.Product.ProductPrices.First().WeightUnit,
                    po.Product.ProductCertificates.Count() > 0),
                page: page, size: size);

            return postList;
        }

        private async Task<List<Ulid>?> GetGardnerIdHaveLocationInRange(string? address, double? range)
        {
            if (address == null || range == null) return null;

            LocationProperties location = new LocationProperties();
            //Get longitude and latitude
            location = await LocationUtil.GetAddressLongLat(address);

             var accountIds = await _unitOfWork.GetRepository<Address>()
                .GetListAsync(
                    predicate: ad => LocationUtil.CheckAddressesInRange(range, location.Lon, location.Lat, ad.Longitude, ad.Latitude),
                    selector: ad => ad.AccountId);

            return accountIds.Distinct().ToList();
        }

        //Get Gardener Post
        public async Task<IPaginate<PostListDTO>> GetGardenerPostList(
            string gardenerId,
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? search = null)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            var specification = new PostSpecification(filterField, filterValue, "Priority", "asc", search);

            var postList = await _unitOfWork.GetRepository<Post>()
                .GetPagingListAsync(
                include: po => po.Include(x => x.PostMedias)
                                 .Include(x => x.Account)
                                 .Include(x => x.Product).ThenInclude(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                 .Include(x => x.Product.ProductCertificates),
                predicate: po => po.GardenerId == gardenerID,
                spec: specification,
                selector: po => new PostListDTO(
                    po.PostId,
                    po.Title,
                    po.Product.ProductPrices.First().Price,
                    po.Product.ProductPrices.First().Currency,
                    po.PostMedias.Where(pm => pm.MediumType == PostMediaTypeEnum.THUMBNAIL.ToString() && pm.PostId == po.PostId).First().MediumUrl,
                    po.Account.Name,
                    po.Account.Avatar,
                    po.CreatedAt,
                    po.Content,
                    po.Status,
                    po.Rating,
                    po.Product.ProductPrices.First().WeightUnit,
                    po.Product.ProductCertificates.Count() > 0),
                page: page, size: size);

            return postList;
        }

        public async Task<List<RetailerPostDTO>> GetPostListForRetailer(
            string? filterField = null,
            string? filterValue = null,
            string? search = null,
            string? address = null,
            double? range = null)
        {
            var specification = new PostSpecification(filterField, filterValue, "Priority", "asc", search);

            List<Ulid>? accountIds = await GetGardnerIdHaveLocationInRange(address, range);

            var postList = await _unitOfWork.GetRepository<Post>()
                .GetListAsync(
                include: po => po.Include(x => x.PostMedias.Where(pm => pm.MediumType.Equals(PostMediaTypeEnum.THUMBNAIL.ToString())))
                                 .Include(x => x.Account).ThenInclude(x => x.Addresses)
                                 .Include(x => x.Product).ThenInclude(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                 .Include(x => x.Product.ProductCertificates),
                predicate: po => po.Status == PostStatusEnum.ACTIVE.ToString() &&
                                 (accountIds != null ? accountIds.Any(x => x == po.GardenerId) : true),
                spec: specification,
                selector: po => new RetailerPostDTO(
                    po.PostId,
                    po.Title,
                    po.Product.ProductPrices.First().Price,
                    po.Product.ProductPrices.First().Currency,
                    po.PostMedias.First().MediumUrl,
                    po.Account.Name,
                    po.Account.Avatar,
                    po.CreatedAt,
                    po.Content,
                    po.Status,
                    po.Rating,
                    po.Product.ProductPrices.First().WeightUnit,
                    po.Product.ProductCertificates.Count() > 0,
                    po.ProductId));

            return postList.ToList();
        }

        public async Task CreatePost(string gardenerId, CreatePostDTO request)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            var post = _mapper.Map<Post>(request);

            //Post Create
            var today = DateTime.UtcNow;
            var gardenerContract = await _unitOfWork.GetRepository<SubscriptionContract>()
                .GetAsync(include: sc => sc.Include(x => x.SubscriptionContractBenefits),
                predicate: sc => sc.GardenerId == gardenerID && sc.StartDate < today && sc.EndDate > today);

            if (gardenerContract == null)
            {
                throw new BadHttpRequestException("User does not have Service Package purchased or Service Package Expired, please purchase Service Package to continue this action");
            }
            else
            {
                var postQuota = gardenerContract.SubscriptionContractBenefits.Where(scb => scb.BenefitType == "POST_QUOTA").First();
                var imageLimit = gardenerContract.SubscriptionContractBenefits.Where(scb => scb.BenefitType == "IMAGE_LIMIT").First();
                var postPriority = gardenerContract.SubscriptionContractBenefits.Where(scb => scb.BenefitType == "POST_PRIORITY").First();

                if (postQuota != null)
                {
                    if (postQuota.RemainingValue <= 0)
                        throw new BadHttpRequestException("The amount of posts created has reached limit, please purchase a new one for continue creating post");
                }

                if (imageLimit != null)
                {
                    if (request.postMediaDTOs != null && request.postMediaDTOs.Where(m => m.MediumType == "IMAGE").Count() > imageLimit.DefaultValue)
                        throw new BadHttpRequestException("The amount of image in the pose exceed the limit allow for your Service Package");
                }

                post.Priority = postPriority != null ? postPriority.DefaultValue : int.MaxValue;
            }

            //Post Media Create
            var postMedia = _mapper.Map<List<PostMedia>>(
                    request.postMediaDTOs,
                    opt => opt.Items["PostId"] = post.PostId
                );

            await _unitOfWork.GetRepository<Post>().InsertAsync(post);
            await _unitOfWork.GetRepository<PostMedia>().InsertRangeAsync(postMedia);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when create post (DB query error)");
        }

        public async Task UpdatePost(string postId, UpdatePostDTO request)
        {
            Ulid postID = Ulid.Parse(postId);
            var post = await _unitOfWork.GetRepository<Post>()
                .GetAsync(predicate: p => p.PostId == postID);

            if (post == null) throw new BadHttpRequestException("Post is not found");

            _mapper.Map(request, post);
            _unitOfWork.GetRepository<Post>().UpdateAsync(post);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update post (DB query error)");
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
                        p.ProductPrices.First().Currency,
                        p.ProductPrices.First().WeightUnit)
                );

            var postMedias = await _unitOfWork.GetRepository<PostMedia>()
                .GetListAsync(predicate: pm => pm.PostId == postID);

            var images = postMedias.Where(pm => pm.MediumType == PostMediaTypeEnum.IMAGE.ToString()).Select(pm => pm.MediumUrl).ToList();
            var video = postMedias.Where(pm => pm.MediumType == PostMediaTypeEnum.VIDEO.ToString()).FirstOrDefault()?.MediumUrl;
            var thumbnail = postMedias.Where(pm => pm.MediumType == PostMediaTypeEnum.THUMBNAIL.ToString()).FirstOrDefault()?.MediumUrl;

            post.ThumbNail = thumbnail;
            post.Video = video;
            post.Images = images;
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
            if (!isSuccess) throw new Exception("Error occurs when updating post status (DB query error)");
        }

        public async Task<List<PostListDTO>> GetRetailerFavoritePost(string retailerId)
        {
            Ulid accountId = Ulid.Parse(retailerId);
            var favoritePosts = await _unitOfWork.GetRepository<Favorite>()
            .GetListAsync(
                include: fav => fav.Include(f => f.Post)
                                       .ThenInclude(p => p.PostMedias.Where(pm => pm.MediumType == PostMediaTypeEnum.THUMBNAIL.ToString()))
                                   .Include(f => f.Post.Product)
                                       .ThenInclude(pr => pr.ProductPrices.Where(pp => pp.IsCurrent))
                                       .Include(f => f.Post.Product.ProductCertificates)
                                   .Include(f => f.Account),
                predicate: fav => fav.RetailerId == accountId && fav.Post.Status == PostStatusEnum.ACTIVE.ToString(),
                selector: fav => new PostListDTO(
                    fav.Post.PostId,
                    fav.Post.Title,
                    fav.Post.Product.ProductPrices.First().Price,
                    fav.Post.Product.ProductPrices.First().Currency,
                    fav.Post.PostMedias.First().MediumUrl,
                    fav.Post.Account.Name,
                    fav.Post.Account.Avatar,
                    fav.Post.CreatedAt,
                    fav.Post.Content,
                    fav.Post.Status,
                    fav.Post.Rating,
                    fav.Post.Product.ProductPrices.First().WeightUnit,
                    fav.Post.Product.ProductCertificates.Count() > 0)
            );

            return favoritePosts.ToList();
        }

        public async Task AddPostToFavorite(string postId, string retailerId)
        {
            Ulid accountId = Ulid.Parse(retailerId);
            Ulid postID = Ulid.Parse(postId);

            var account = await _unitOfWork.GetRepository<Account>().GetAsync(predicate: ac => ac.AccountId == accountId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            var post = await _unitOfWork.GetRepository<Post>().GetAsync(predicate: po => po.PostId == postID);
            if (post == null) throw new BadHttpRequestException("Post is not found");

            Favorite newFavPost = new Favorite
            {
                FavoriteId = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow,
                RetailerId = accountId,
                PostId = postID
            };

            await _unitOfWork.GetRepository<Favorite>().InsertAsync(newFavPost);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when add post to favorite (DB query error)");
        }

        public async Task RemovePostFromFavorite(string postId, string retailerId)
        {
            Ulid accountId = Ulid.Parse(retailerId);
            Ulid postID = Ulid.Parse(postId);

            var account = await _unitOfWork.GetRepository<Account>().GetAsync(predicate: ac => ac.AccountId == accountId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            var post = await _unitOfWork.GetRepository<Post>().GetAsync(predicate: po => po.PostId == postID);
            if (post == null) throw new BadHttpRequestException("Post is not found");

            var favorite = await _unitOfWork.GetRepository<Favorite>()
                .GetAsync(predicate: fa => fa.PostId == postID && fa.RetailerId == accountId);
            if (favorite == null) throw new BadHttpRequestException("Favorite post is not found");

            _unitOfWork.GetRepository<Favorite>().DeleteAsync(favorite);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when delete post from favorite (DB query error)");
        }
    }
}
