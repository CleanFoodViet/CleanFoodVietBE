using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public record CreateAppointmentDTO
    {
        public Ulid GardenerId { get; set; }
        public Ulid RetailerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Subject { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
    }
}
