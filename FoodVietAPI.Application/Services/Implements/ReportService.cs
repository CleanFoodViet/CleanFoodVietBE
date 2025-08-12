using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ReportDTOs;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Enums.ReportEnums;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Security.Principal;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ReportService : BaseService<ReportService>, IReportService
    {
        public ReportService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ReportService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<ReportListDTO>> GetReportList(int page, int size)
        {
            var productList = await _unitOfWork.GetRepository<Report>()
                .GetPagingListAsync(
                    selector: rp => new ReportListDTO(
                        rp.ReportId,
                        rp.ReportType,
                        rp.TargetType,
                        rp.Subject,
                        rp.Description,
                        rp.Severity,
                        rp.Status,
                        rp.CreatedAt),
                    page: page, size: size);

            return productList;
        }

        public async Task<ReportDTO> GetReportInformation(string reportId)
        {
            Ulid rpId = Ulid.Parse(reportId);
            var product = await _unitOfWork.GetRepository<Report>()
                .GetAsync(
                    predicate: rp => rp.ReportId == rpId,
                    include: rp => rp.Include(x => x.Account).ThenInclude(x => x.Role),
                    selector: rp => new ReportDTO(
                        rp.ReportId,
                        rp.ReportType,
                        rp.TargetId,
                        rp.TargetType,
                        rp.Subject,
                        rp.Description,
                        rp.Severity,
                        rp.Status,
                        rp.CreatedAt,
                        rp.UpdatedAt,
                        rp.Account.PhoneNumber,
                        rp.Account.Avatar,
                        rp.Account.Role.Name
                    ));

            return product;
        }

        public async Task CreateReport(CreateReportDTO createReportData)
        {
            var newReport = _mapper.Map<Report>(createReportData);
            await _unitOfWork.GetRepository<Report>().InsertAsync(newReport);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Report (DB query error)");
            }
        }

        public async Task CreateUserReport(CreateUserReportDTO createReportData)
        {
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: a => a.PhoneNumber == createReportData.PhoneNumber);
            if (account == null) throw new BadHttpRequestException("Account is not found, please check the phone number again");

            var newReport = _mapper.Map<Report>(createReportData);
            newReport.TargetId = account.AccountId;

            await _unitOfWork.GetRepository<Report>().InsertAsync(newReport);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Report (DB query error)");
            }
        }

        public async Task UpdateReportStatus(string reportId, string status)
        {
            Ulid rpId = Ulid.Parse(reportId);
            Report report = await _unitOfWork.GetRepository<Report>()
                .GetAsync(predicate: rp => rp.ReportId == rpId);
            if (report == null) throw new BadHttpRequestException("Report is not found");

            if (Enum.TryParse<ReportStatusEnum>(status.ToUpper(), out var result))
            {
                report.Status = result.ToString();
            }
            else
            {
                throw new BadHttpRequestException($"Report status {status.ToUpper()} is not existed");
            }

            report.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Report>().UpdateAsync(report);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when updating report status (DB query error)");
        }
    }
}
