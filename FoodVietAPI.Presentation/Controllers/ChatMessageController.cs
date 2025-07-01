using CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ZstdSharp.Unsafe;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ChatMessageController : BaseController<ChatMessageController>
    {
        private readonly IChatMessageService _chatMessageService;
        public ChatMessageController(ILogger<ChatMessageController> logger, IChatMessageService chatMessageService) : base(logger)
        {
            _chatMessageService = chatMessageService;
        }

        [HttpPost(ApiEndpointConstant.ChatMessage.ChatMessagesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Save a message between Gardener and Retailer")]
        public async Task<IActionResult> SaveMessage([FromBody]ChatMessageDTO request)
        {
            await _chatMessageService.SaveMessage(request);
            return Ok("Save message successfully");
        }
    }
}
