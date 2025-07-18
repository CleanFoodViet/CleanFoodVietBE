using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class NotificationService : BaseService<NotificationService>, INotificationService
    {
        public NotificationService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<NotificationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<NotificationDTO>> GetNotificationList(string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var notificationList = await _unitOfWork.GetRepository<Notification>()
                .GetListAsync(
                    predicate: n => n.AccountId == accId,
                    selector: n => new NotificationDTO
                    {
                        NotificationId = n.NotificationId,
                        Sender = n.Sender,
                        CreatedAt = n.CreatedAt,
                        IsRead = n.IsRead,
                        Link = n.Link,
                        Message = n.Message
                    }
                );

            return notificationList.ToList();
        }

        public async Task CreateNotification(CreateNotificationDTO request)
        {
            Notification newNoti = _mapper.Map<Notification>(request);
            await _unitOfWork.GetRepository<Notification>().InsertAsync(newNoti);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when create notification (DB query error)");
        }

        public async Task ReadNotification(string notificationId)
        {
            Ulid notiId = Ulid.Parse(notificationId);
            Notification unreadNotification = await _unitOfWork.GetRepository<Notification>()
                .GetAsync(predicate: n => n.NotificationId == notiId);

            if (unreadNotification == null) throw new BadHttpRequestException("Notification is not found");

            unreadNotification.IsRead = true;
            _unitOfWork.GetRepository<Notification>().UpdateAsync(unreadNotification);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update notification to read (DB query error)");
        }
    }
}
