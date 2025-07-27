using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.DTOs.StatisticDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IStatisticService
    {
        Task<GardenerStatisticDTO> GetGardenerDashboard(string gardenerId);
        Task<List<MonthOrderStatistic>> GetGardenerYearlyOrderAmount(string gardenerId);
        Task<List<ScheduleAppointmentDTO>> GetInMonthUpcommingAppointment(string gardenerId);
    }
}
