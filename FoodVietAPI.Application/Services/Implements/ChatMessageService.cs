using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ChatMessageService : BaseService<ChatMessageService>, IChatMessageService
    {
        public ChatMessageService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ChatMessageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task SaveMessage(ChatMessageDTO request)
        {
            ChatMessage message = _mapper.Map<ChatMessage>(request);

            await _unitOfWork.GetRepository<ChatMessage>().InsertAsync(message);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when saving message (DB query error)");
        }
    }
}
