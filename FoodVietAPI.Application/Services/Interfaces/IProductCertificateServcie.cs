using CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductCertificateServcie
    {
        Task<List<GetProductCertificateDTO>> GetProductCertificate(string productId);

        Task CreateProductCertificate(string productId, ProductCertificateDTO data);
    }
}
