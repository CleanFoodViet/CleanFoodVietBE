using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServiceFeatureService : BaseService<ServiceFeatureService>, IServiceFeatureService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceFeatureService(
            IUnitOfWork<CleanFoodVietDbContext> unitOfWork,
            ILogger<ServiceFeatureService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPaginate<ServiceFeatureDTO>> GetServiceFeatureList(int page, int size)
        {
            var paginatedFeatures = await _unitOfWork.GetRepository<ServiceFeature>().GetPagingListAsync(
                include: null,  // No eager loading needed here
                selector: sf => new ServiceFeatureDTO(
                    sf.ServiceFeatureId,
                    sf.ServiceFeatureName,
                    sf.Description,
                    sf.DefaultValue
                ),
                page: page,
                size: size
            );
            return paginatedFeatures;
        }

        public async Task<ServiceFeatureDTO> CreateServiceFeature(CreateServiceFeatureDTO createDto)
        {
            // Map incoming DTO to entity
            var newFeature = _mapper.Map<ServiceFeature>(createDto);

            // Insert into the repository
            await _unitOfWork.GetRepository<ServiceFeature>().InsertAsync(newFeature);

            // Commit changes
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Service Feature.");
            }

            // Map back to DTO and return the created record
            var resultDto = _mapper.Map<ServiceFeatureDTO>(newFeature);
            return resultDto;
        }
    }
}
