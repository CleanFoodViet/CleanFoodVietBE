using CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductCertificateServcie
    {
        Task<List<GetProductCertificateDTO>> GetProductCertificate(string productId);

        Task CreateProductCertificate(string productId, ProductCertificateDTO data);
    }
}
