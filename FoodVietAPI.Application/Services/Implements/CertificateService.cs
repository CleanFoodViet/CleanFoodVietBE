using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class CertificateService : BaseService<CertificateService>, ICertificateService
    {
        public CertificateService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<CertificateService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<GetCertificateDTO>> GetGardenerCertificateList(string gardenerId)
        {
            Ulid accountId = Ulid.Parse(gardenerId);

            var gardener = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accountId);
            if (gardener == null) throw new BadHttpRequestException("Gardener is not found");

            var certificates = await _unitOfWork.GetRepository<Certificate>()
                .GetListAsync(
                    predicate: ce => ce.GardenerId == accountId,
                    selector: ce => new GetCertificateDTO
                    {
                        CertificateId = ce.CertificateId,
                        ExpiryDate = ce.ExpiryDate,
                        ImageUrl = ce.ImageUrl,
                        IssueDate = ce.IssueDate,
                        IssuingAuthority = ce.IssuingAuthority,
                        Name = ce.Name,
                        Status = ce.Status
                    }
                );

            return certificates.ToList();
        }

        public async Task<GetCertificateDTO> GetGardenerCertificateDetail(string certificateId)
        {
            Ulid cerId = Ulid.Parse(certificateId);

            var certificate = await _unitOfWork.GetRepository<Certificate>()
                .GetAsync(
                    predicate: ce => ce.CertificateId == cerId,
                    selector: ce => new GetCertificateDTO
                    {
                        CertificateId = ce.CertificateId,
                        ExpiryDate = ce.ExpiryDate,
                        ImageUrl = ce.ImageUrl,
                        IssueDate = ce.IssueDate,
                        IssuingAuthority = ce.IssuingAuthority,
                        Name = ce.Name,
                        Status = ce.Status
                    });
            if (certificate == null) throw new BadHttpRequestException("Certificate is not found");

            return certificate;
        }

        public async Task CreateCertificate(CreateCertificateDTO createData, string gardenerId)
        {
            Ulid accountId = Ulid.Parse(gardenerId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accountId);
            if (account == null) throw new BadHttpRequestException("Gardener is not found");

            if (createData.IssueDate >= createData.ExpiryDate) throw new BadHttpRequestException("Invalid date range: IssueDate cannot be equal or latter than ExpiredDate");

            Certificate newCertificate = _mapper.Map<Certificate>(createData);
            newCertificate.GardenerId = accountId;

            await _unitOfWork.GetRepository<Certificate>().InsertAsync(newCertificate);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when create certificate (DB query error)");
        }

        public async Task UpdateCertificate(UpdateCertificateDTO updateData, string certificateId)
        {
            Ulid certificateID = Ulid.Parse(certificateId);
            var certificate = await _unitOfWork.GetRepository<Certificate>()
                .GetAsync(predicate: ce => ce.CertificateId == certificateID);
            if (certificate == null) throw new BadHttpRequestException("Certificate is not found");

            certificate.Name = String.IsNullOrEmpty(updateData.Name) ? certificate.Name : updateData.Name; 
            certificate.IssuingAuthority = String.IsNullOrEmpty(updateData.IssuingAuthority) ? certificate.IssuingAuthority : updateData.IssuingAuthority; 
            certificate.Status = String.IsNullOrEmpty(updateData.Status) ? certificate.Status : updateData.Status;
            certificate.IssueDate = updateData.IssueDate;
            certificate.ExpiryDate = updateData.ExpiryDate;

            _unitOfWork.GetRepository<Certificate>().UpdateAsync(certificate);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when update certificate (DB query error)");
        }

        public async Task DeleteCertificate(string certificateId)
        {
            Ulid certificateID = Ulid.Parse(certificateId);
            var certificate = await _unitOfWork.GetRepository<Certificate>()
                .GetAsync(predicate: ce => ce.CertificateId == certificateID);
            if (certificate == null) throw new BadHttpRequestException("Certificate is not found");

            _unitOfWork.GetRepository<Certificate>().DeleteAsync(certificate);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when delete certificate (DB query error)");
        }
    }
}
