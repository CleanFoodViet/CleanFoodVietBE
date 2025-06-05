using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO;
using CleanFoodVietAPI.Data.Paginate;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IServiceFeatureService
    {
        // Added optional parameters for filtering and sorting.
        Task<IPaginate<ServiceFeatureDTO>> GetServiceFeatureList(
            int page,
            int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc");

        Task<ServiceFeatureDTO> CreateServiceFeature(CreateServiceFeatureDTO createDto);

        // New update method (using PATCH)
        Task<ServiceFeatureDTO> UpdateServiceFeature(Ulid id, UpdateServiceFeatureDTO updateDto);
    }
}
