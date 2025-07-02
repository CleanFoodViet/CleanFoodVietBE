using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<List<GetCertificateDTO>> GetGardenerCertificateList(string gardenerId);

        Task<GetCertificateDTO> GetGardenerCertificateDetail(string certificateId);

        Task CreateCertificate(CertificateDTO createData, string gardenerId);

        Task UpdateCertificate(CertificateDTO updateData, string certificateId);

        Task DeleteCertificate(string certificateId);
    }
}
