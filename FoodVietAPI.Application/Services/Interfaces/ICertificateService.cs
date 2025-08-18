using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<List<GetCertificateDTO>> GetGardenerCertificateList(string gardenerId);

        Task<GetCertificateDTO> GetGardenerCertificateDetail(string certificateId);

        Task CreateCertificate(CreateCertificateDTO createData, string gardenerId);

        Task UpdateCertificate(UpdateCertificateDTO updateData, string certificateId);

        Task DeleteCertificate(string certificateId);
    }
}
