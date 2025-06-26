using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentListDTO>> GetAppointmentList(string accountId);
        Task<AppointmentDTO> GetAppointmentDetail(string appointmentId);
        Task CreateAppointment(CreateAppointmentDTO createData);
        Task UpdateAppointment(string appointmentId, UpdateAppointmentDTO updateData);
    }
}
