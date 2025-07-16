using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public record ScheduleAppointmentDTO
    {
        public Ulid AppointmentId { get; set; }
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string AppointmentType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
    }
}
