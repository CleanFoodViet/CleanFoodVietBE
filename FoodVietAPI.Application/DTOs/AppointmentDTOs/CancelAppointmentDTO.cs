using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public class CancelAppointmentDTO
    {
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
    }
}
