using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentListDTO>> GetAppointmentList(string accountId);
        Task<AppointmentDTO> GetAppointmentDetail(string appointmentId);
        Task CreateAppointment(CreateAppointmentDTO createData);
        Task UpdateAppointment(string appointmentId, UpdateAppointmentDTO updateData);
        Task UpdateAppointmentStatus(string appointmentId, string status);
        Task CancelOrRejectAppointment(string appointmentId, CancelOrRejectAppointmentDTO cancelData, string status);

        Task<IPaginate<GetRequestAppointmentDTO>> GetRequestAppointment(string gardenerId, int page, int size);
        Task<List<ScheduleAppointmentDTO>> GetScheduleAppointments(string gardenerId);
    }
}
