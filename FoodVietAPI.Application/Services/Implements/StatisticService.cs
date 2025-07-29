using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.DTOs.StatisticDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AppointmentEnums;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class StatisticService : BaseService<StatisticService>, IStatisticService
    {
        public StatisticService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<StatisticService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<GardenerStatisticDTO> GetGardenerDashboard(string gardenerId)
        {
            int currentMonth = DateTime.UtcNow.Month;

            Ulid gardenerID = Ulid.Parse(gardenerId);
            var orderStatisitic = await _unitOfWork.GetRepository<Order>()
                .GetListAsync(
                    include: os => os.Include(x => x.OrderDeliveries),
                    predicate: os => os.GardenerId == gardenerID && os.CreatedAt.Month == currentMonth
                );

            DateTime now = DateTime.UtcNow;
            DateTime endDate = now.AddDays(30);

            var appointmentStatisic = await _unitOfWork.GetRepository<Appointment>()
                .GetListAsync(predicate: aps => aps.GardenerId == gardenerID && aps.AppointmentDate >= now && aps.AppointmentDate <= endDate);

            var productStatistic = await _unitOfWork.GetRepository<Product>()
                .GetListAsync(predicate: pds => pds.GardenerId == gardenerID);

            var postStatistic = await _unitOfWork.GetRepository<Post>()
                .GetListAsync(predicate: ps => ps.GardenerId == gardenerID);

            GardenerStatisticDTO monthlyStatistic = new GardenerStatisticDTO
            {
                TotalOrders = orderStatisitic.Count(),
                TotalOrderDeliveries = orderStatisitic.SelectMany(os => os.OrderDeliveries).Count(),
                TotalAppointments = appointmentStatisic.Count(),
                TotalPosts = postStatistic.Count(),
                TotalProducts = productStatistic.Count(),
                OrderList = orderStatisitic.Select(os => new GardenerOrderStatistic
                {
                    CreatedAt = os.CreatedAt,
                    OrderId = os.OrderId,
                    PaymentMethod = os.PaymentMethod,
                    Status = os.Status 
                }).ToList(),
            };

            return monthlyStatistic;
        }

        public async Task<List<MonthOrderStatistic>> GetGardenerYearlyOrderAmount(string gardenerId)
        {
            int currentYear = DateTime.UtcNow.Year;

            Ulid gardenerID = Ulid.Parse(gardenerId);
            var orderStatisitic = await _unitOfWork.GetRepository<Order>()
                .GetListAsync(
                    predicate: os => os.GardenerId == gardenerID && os.CreatedAt.Year == currentYear
                );

            var ordersPerMonth = orderStatisitic
            .GroupBy(o => o.CreatedAt.Month)
            .Select(g => new MonthOrderStatistic
            {
                Month = g.Key, // This will be 1 to 12
                Amount = g.Count()
            })
            .OrderBy(g => g.Month)
            .ToList();

            return ordersPerMonth;
        }

        public async Task<List<ScheduleAppointmentDTO>> GetInMonthUpcommingAppointment(string gardenerId)
        {
            int currentMonth = DateTime.UtcNow.Month;

            Ulid accountId = Ulid.Parse(gardenerId);
            var appointmentList = await _unitOfWork.GetRepository<Appointment>()
                .GetListAsync(
                    include: ap => ap.Include(x => x.Retailer),
                    predicate: ap => ap.GardenerId == accountId && ap.AppointmentDate.Month == currentMonth && 
                                     ap.Status != AppointmentStatusEnum.PENDING.ToString() &&
                                     ap.Status != AppointmentStatusEnum.REJECTED.ToString(),
                    selector: ap => new ScheduleAppointmentDTO
                    {
                        Status = ap.Status,
                        Subject = ap.Subject,
                        AppointmentDate = ap.AppointmentDate,
                        AppointmentId = ap.AppointmentId,
                        AppointmentType = ap.AppointmentType,
                        Description = ap.Description,
                        AccountName = ap.Retailer.Name,
                        AccountAvatar = ap.Retailer.Avatar,
                        AccountPhoneNumber = ap.Retailer.PhoneNumber,
                        StartTime = ap.AppointmentDate.ToString("HH:mm"),
                        EndTime = ap.AppointmentDate.AddMinutes(ap.Duration).ToString("HH:mm")
                    }
                );

            return appointmentList.ToList();
        }
    }
}
    