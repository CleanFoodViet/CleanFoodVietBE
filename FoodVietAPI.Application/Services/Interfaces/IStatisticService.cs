using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.DTOs.StatisticDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IStatisticService
    {
        Task<GardenerStatisticDTO> GetGardenerDashboard(string gardenerId);
        Task<List<MonthOrderStatistic>> GetGardenerYearlyOrderAmount(string gardenerId, int? year);
        Task<List<ScheduleAppointmentDTO>> GetInMonthUpcommingAppointment(string gardenerId);
    }
}
