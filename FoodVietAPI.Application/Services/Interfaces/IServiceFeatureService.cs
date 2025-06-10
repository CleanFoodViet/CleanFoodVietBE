using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IServiceFeatureService
    {
        Task<IPaginate<ServiceFeatureDTO>> GetServiceFeatureList(
            int page,
            int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null);

        Task<ServiceFeatureDTO> CreateServiceFeature(CreateServiceFeatureDTO createDto);

        Task<ServiceFeatureDTO> UpdateServiceFeature(Ulid id, UpdateServiceFeatureDTO updateDto);

        Task<ServiceFeatureDTO> SoftDeleteServiceFeature(Ulid id);
    }
}
