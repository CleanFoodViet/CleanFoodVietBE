using AutoMapper;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CleanFoodVietAPI.Application.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<CleanFoodVietDbContext> _unitOfWork;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;
        protected ILogger<T> _logger;
        public BaseService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<T> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected string GetUsernameFromJwt()
        {
            string username = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return username;
        }

        protected string GetRoleFromJwt()
        {
            string role = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!;
            return role;
        }
    }
}
