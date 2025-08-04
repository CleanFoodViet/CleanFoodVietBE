using CleanFoodVietAPI.Application.DTOs.ReportDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IPaginate<ReportListDTO>> GetReportList(int page, int size);
        Task<ReportDTO> GetReportInformation(string reportId);
        Task CreateReport(CreateReportDTO createReportData);
        Task CreateUserReport(CreateUserReportDTO createReportData);
        Task UpdateReportStatus(string reportId, string status);
    }
}
