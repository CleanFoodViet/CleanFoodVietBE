using AutoMapper;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class SystemLogService : BaseService<SystemLogService>, ISystemLogService
    {
        public SystemLogService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<SystemLogService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
    }
}
