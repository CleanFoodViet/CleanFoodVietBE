using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ProductCertificateServcie : BaseService<ProductCertificateServcie>, IProductCertificateServcie
    {
        public ProductCertificateServcie(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ProductCertificateServcie> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<GetProductCertificateDTO>> GetProductCertificate(string productId)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>().GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            var certificates = await _unitOfWork.GetRepository<ProductCertificate>()
                .GetListAsync(
                    predicate: pc => pc.ProductId == productID,
                    selector: pc => new GetProductCertificateDTO
                    {
                        CertificateName = pc.CertificateName,
                        CertificateNumber = pc.CertificateNumber,
                        ExpirationDate = pc.ExpirationDate,
                        ImageUrl = pc.ImageUrl,
                        IssuedDate = pc.IssuedDate,
                        IssuingOrganization = pc.IssuingOrganization,
                        ProductCertificateId = pc.ProductCertificateId
                    });

            return certificates.ToList();
        }

        public async Task CreateProductCertificate(string productId, ProductCertificateDTO data)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>().GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            var certificate = _mapper.Map<ProductCertificate>(
                    data,
                    opt => opt.Items["ProductId"] = productID
                );

            await _unitOfWork.GetRepository<ProductCertificate>().InsertAsync(certificate);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when insert product certificate to DB (DB query error)");
        }
    }
}
