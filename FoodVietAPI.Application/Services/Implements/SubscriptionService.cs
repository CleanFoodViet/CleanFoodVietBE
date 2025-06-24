using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class SubscriptionService : BaseService<SubscriptionService>, ISubscriptionService
    {
        public SubscriptionService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<SubscriptionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<SubscriptionContractListDTO>> GetSubscriptionContractList(int page, int size)
        {
            var contractList = await _unitOfWork.GetRepository<SubscriptionContract>()
                .GetPagingListAsync(
                include: sc => sc.Include(x => x.Account).Include(x => x.ServicePackage),
                selector: sc => new SubscriptionContractListDTO(
                    sc.SubscriptionId,
                    sc.Status,
                    sc.SubscriptionType,
                    sc.Account.PhoneNumber,
                    sc.ServicePackage.PackageName,
                    sc.ServicePackage.Price),
                page: page, size: size
                );

            return contractList;
        }

        public async Task<SubscriptionContractDTO> GetSubscriptionContractInformation(string id)
        {
            Ulid subsdcriptionId = Ulid.Parse(id);
            var subscriptionContract = await _unitOfWork.GetRepository<SubscriptionContract>()
                .GetAsync(
                include: sc => sc.Include(x => x.Account).Include(x => x.ServicePackage),
                predicate: sc => sc.SubscriptionId == subsdcriptionId,
                selector: sc => new SubscriptionContractDTO(
                    sc.SubscriptionId,
                    sc.StartDate,
                    sc.EndDate,
                    sc.Status,
                    sc.SubscriptionType,
                    sc.CreatedAt,
                    sc.Account.PhoneNumber,
                    sc.ServicePackage.PackageName,
                    sc.ServicePackage.Price));

            return subscriptionContract;
        }
    }
}
