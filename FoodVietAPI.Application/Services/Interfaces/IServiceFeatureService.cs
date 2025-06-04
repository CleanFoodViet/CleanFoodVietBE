using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO;
using CleanFoodVietAPI.Data.Paginate;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IServiceFeatureService
    {
        Task<IPaginate<ServiceFeatureDTO>> GetServiceFeatureList(int page, int size);
        Task<ServiceFeatureDTO> CreateServiceFeature(CreateServiceFeatureDTO createDto);
    }
}
