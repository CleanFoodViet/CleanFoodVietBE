using CleanFoodVietAPI.Application.DTOs.OrderDTOs;
using Stripe.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.StatisticDTOs
{
    public record GardenerStatisticDTO
    {
        //General Data (base on month)
        public int TotalOrders { get; set; }
        public int TotalOrderDeliveries { get; set; }
        public int TotalAppointments{ get; set; }
        public int TotalProducts{ get; set; }
        public int TotalPosts{ get; set; }

        //Orders in a Year (for 2 graph: Month in Year and status)
        public List<GardenerOrderStatistic> OrderList { get; set; } = new List<GardenerOrderStatistic>();
    }

    //Yearly Order Statistic
    public record MonthOrderStatistic
    {
        public int Month { get; set; }
        public int Amount { get; set; }
    }

    public record GardenerOrderStatistic
    {
        public Ulid OrderId { get; set; }
        public string Status { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    ////Upcomming appointment base on month
    //public record GardenerAppointmentStatistic
    //{
    //    public Ulid AppointmentId { get; set; }
    //    public string Subject { get; set; } = null!;
    //    public string AppointmentType { get; set; } = null!;
    //    public string Status { get; set; } = null!;
    //    public DateTime AppointmentDate { get; set; }
    //    public string AccountName { get; set; } = null!;
    //    public string AccountPhoneNumber { get; set; } = null!;
    //    public string StartTime { get; set; } = null!;
    //    public string EndTime { get; set; } = null!;
    //}
}
