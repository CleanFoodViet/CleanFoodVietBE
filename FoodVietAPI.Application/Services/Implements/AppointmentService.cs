using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AppointmentEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class AppointmentService : BaseService<AppointmentService>, IAppointmentService
    {
        public AppointmentService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<AppointmentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<AppointmentListDTO>> GetAppointmentList(string accountId)
        {
            Ulid accountID = Ulid.Parse(accountId);
            var appointmentList = await _unitOfWork.GetRepository<Appointment>()
                .GetListAsync(
                    predicate: ap => ap.RetailerId == accountID || ap.GardenerId == accountID,
                    selector: ap => new AppointmentListDTO
                    {
                        GardenerId = ap.GardenerId,
                        RetailerId = ap.RetailerId,
                        AppointmentDate = ap.AppointmentDate,
                        AppointmentId = ap.AppointmentId,
                        AppointmentType = ap.AppointmentType,
                        CancellationReason = ap.CancellationReason,
                        CancelledBy = ap.CancelledBy,
                        CreatedAt = ap.CreatedAt,
                        Location = ap.Location,
                        Status = ap.Status,
                        Subject = ap.Subject,
                        UpdatedAt = ap.UpdatedAt
                    }
                );
            return appointmentList.ToList();
        }

        public async Task<AppointmentDTO> GetAppointmentDetail(string appointmentId)
        {
            Ulid appointmentID = Ulid.Parse(appointmentId);
            var appointment = await _unitOfWork.GetRepository<Appointment>()
                .GetAsync(
                    predicate: ap => ap.AppointmentId == appointmentID,
                    selector: ap => new AppointmentDTO
                    {
                        GardenerId = ap.GardenerId,
                        RetailerId = ap.RetailerId,
                        AppointmentDate = ap.AppointmentDate,
                        AppointmentId = ap.AppointmentId,
                        AppointmentType = ap.AppointmentType,
                        CancellationReason = ap.CancellationReason,
                        CancelledBy = ap.CancelledBy,
                        CreatedAt = ap.CreatedAt,
                        Location = ap.Location,
                        Status = ap.Status,
                        Subject = ap.Subject,
                        UpdatedAt = ap.UpdatedAt,
                        Description = ap.Description,
                        Duration = ap.Duration
                    }
                );

            return appointment;
        }

        public async Task CreateAppointment(CreateAppointmentDTO createData)
        {
            var newAppointment = _mapper.Map<Appointment>(createData);
            await _unitOfWork.GetRepository<Appointment>().InsertAsync(newAppointment);
            bool isSucces = await _unitOfWork.CommitAsync() > 0;
            if (!isSucces) throw new Exception("Error occur when create appointment (DB query error)");
        }
        
        public async Task UpdateAppointment(string appointmentId, UpdateAppointmentDTO updateData)
        {
            Ulid appointmentID = Ulid.Parse(appointmentId);
            var appointment = await _unitOfWork.GetRepository<Appointment>()
                .GetAsync(predicate: ap => ap.AppointmentId == appointmentID);

            if (appointment == null) throw new BadHttpRequestException("Appointment is not found");

            _mapper.Map(updateData, appointment);
            _unitOfWork.GetRepository<Appointment>().UpdateAsync(appointment);
            bool isSucces = await _unitOfWork.CommitAsync() > 0;
            if (!isSucces) throw new Exception("Error occur when update appointment (DB query error)");
        }

        public async Task UpdateAppointmentStatus(string appointmentId, string status)
        {
            Ulid appointmentID = Ulid.Parse(appointmentId);

            Appointment appointment = await _unitOfWork.GetRepository<Appointment>()
                .GetAsync(predicate: app => app.AppointmentId == appointmentID);
            if (appointment == null) throw new BadHttpRequestException("Appointment is not found");

            if (Enum.TryParse<AppointmentStatusEnum>(status.ToUpper(), out var result))
            {
                appointment.Status = result.ToString();
            }
            else
            {
                throw new BadHttpRequestException("Invalid appointment status");
            }

            _unitOfWork.GetRepository<Appointment>().UpdateAsync(appointment);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update appointment status (DB query error)");
        }

        public async Task CancelAppointment(string appointmentId, CancelAppointmentDTO cancelData)
        {
            Ulid appointmentID = Ulid.Parse(appointmentId);

            Appointment appointment = await _unitOfWork.GetRepository<Appointment>()
                .GetAsync(predicate: app => app.AppointmentId == appointmentID);
            if (appointment == null) throw new BadHttpRequestException("Appointment is not found");

            appointment.CancellationReason = cancelData.CancellationReason;
            appointment.CancelledBy = cancelData.CancelledBy;

            _unitOfWork.GetRepository<Appointment>().UpdateAsync(appointment);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update appointment status (DB query error)");
        }

        public async Task<IPaginate<GetRequestAppointmentDTO>> GetRequestAppointment(string gardenerId)
        {
            Ulid accountId = Ulid.Parse(gardenerId);
            var appointmentList = await _unitOfWork.GetRepository<Appointment>()
                .GetPagingListAsync(
                    include: ap => ap.Include(x => x.Retailer),
                    predicate: ap => ap.GardenerId == accountId && ap.Status == AppointmentStatusEnum.PENDING.ToString(),
                    selector: ap => new GetRequestAppointmentDTO
                    {
                        AppointmentDate = ap.AppointmentDate,
                        AppointmentId = ap.AppointmentId,
                        AppointmentType = ap.AppointmentType,
                        Avatar = ap.Retailer.Avatar,
                        Description = ap.Description,
                        Duration = ap.Duration,
                        Location = ap.Location,
                        Status = ap.Status,
                        Subject = ap.Subject,
                        PhoneNumber = ap.Retailer.PhoneNumber,
                        RetailerName = ap.Retailer.Name
                    }
                );

            return appointmentList;
        }

        public async Task<List<ScheduleAppointmentDTO>> GetScheduleAppointments(string gardenerId)
        {
            Ulid accountId = Ulid.Parse(gardenerId);
            var appointmentList = await _unitOfWork.GetRepository<Appointment>()
                .GetListAsync(
                    include: ap => ap.Include(x => x.Retailer),
                    predicate: ap => ap.GardenerId == accountId &&
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
                    }
                );

            return appointmentList.ToList();
        }
    }
}
