using AutoMapper;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class AppointmentService : BaseService<AppointmentService>, IAppointmentService
    {
        public AppointmentService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<AppointmentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
    }
}
