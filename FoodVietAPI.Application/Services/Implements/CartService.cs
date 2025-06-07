using AutoMapper;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class CartService : BaseService<CartService>, ICartService
    {
        public CartService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<CartService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
    }
}
