using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServiceFeatureService : BaseService<ServiceFeatureService>, IServiceFeatureService
    {
        public ServiceFeatureService(
            IUnitOfWork<CleanFoodVietDbContext> unitOfWork,
            ILogger<ServiceFeatureService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Existing GetServiceFeatureList and CreateServiceFeature are here...
        public async Task<IPaginate<ServiceFeatureDTO>> GetServiceFeatureList(
            int page,
            int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc")
        {
            // Build the specification from incoming parameters.
            var specification = new ServiceFeatureSpecification(filterField, filterValue, sortField, sortOrder);

            // Retrieve the paginated list using the specification.
            var featuresPage = await _unitOfWork.GetRepository<ServiceFeature>()
                                                .GetPagingListAsync(spec: specification,
                                                selector: sf => new ServiceFeatureDTO(
                                                    sf.ServiceFeatureId,
                                                    sf.ServiceFeatureName,
                                                    sf.Description,
                                                    sf.DefaultValue),
                                                page: page, size: size);

            return featuresPage;
        }

        public async Task<ServiceFeatureDTO> CreateServiceFeature(CreateServiceFeatureDTO createDto)
        {
            var newFeature = _mapper.Map<ServiceFeature>(createDto);
            newFeature.Status = ServiceFeatureStatusEnum.ACTIVE.ToString();
            await _unitOfWork.GetRepository<ServiceFeature>().InsertAsync(newFeature);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Service Feature.");
            }
            var resultDto = _mapper.Map<ServiceFeatureDTO>(newFeature);
            return resultDto;
        }

        // New update method using PATCH
        public async Task<ServiceFeatureDTO> UpdateServiceFeature(Ulid id, UpdateServiceFeatureDTO updateDto)
        {
            // Retrieve the existing ServiceFeature entity by id using the generic overload.
            var repository = _unitOfWork.GetRepository<ServiceFeature>();
            var existingFeature = await repository.GetAsync<ServiceFeature>(
                selector: x => x,
                predicate: x => x.ServiceFeatureId == id);

            if (existingFeature == null)
            {
                throw new Exception("Service Feature not found.");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(updateDto.ServiceFeatureName))
            {
                existingFeature.ServiceFeatureName = updateDto.ServiceFeatureName;
            }
            if (!string.IsNullOrEmpty(updateDto.Description))
            {
                existingFeature.Description = updateDto.Description;
            }
            if (!string.IsNullOrEmpty(updateDto.DefaultValue))
            {
                existingFeature.DefaultValue = updateDto.DefaultValue;
            }
            if (!string.IsNullOrEmpty(updateDto.Status))
            {
                // If a soft-delete is desired, we expect "DISABLE" (case-insensitive).
                if (updateDto.Status.Equals("DISABLE", StringComparison.OrdinalIgnoreCase))
                {
                    existingFeature.Status = ServiceFeatureStatusEnum.INACTIVE.ToString();
                }
                else
                {
                    try
                    {
                        existingFeature.Status = Enum.Parse<ServiceFeatureStatusEnum>(updateDto.Status, true).ToString();
                    }
                    catch
                    {
                        throw new Exception("Invalid status value provided.");
                    }
                }
            }

            // Save the updates.
            repository.UpdateAsync(existingFeature);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while updating the Service Feature.");
            }

            var updatedDto = _mapper.Map<ServiceFeatureDTO>(existingFeature);
            return updatedDto;
        }
    }
}
