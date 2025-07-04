using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AppointmentController : BaseController<AppointmentController>
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService) : base(logger)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet(ApiEndpointConstant.Account.AccountAppointmentEndpoint)]
        [ProducesResponseType(typeof(List<AppointmentListDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all appointments of account")]
        public async Task<IActionResult> GetAccountAppointmentList([FromRoute] string accountId)
        {
            var res = await _appointmentService.GetAppointmentList(accountId);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Appointment.AppointmentEndpoint)]
        [ProducesResponseType(typeof(AppointmentDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get appointment detail of account")]
        public async Task<IActionResult> GetAppointment([FromRoute] string id)
        {
            var res = await _appointmentService.GetAppointmentDetail(id);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Appointment.AppointmentsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create appointment for account")]
        public async Task<IActionResult> CreateAppointment([FromBody]CreateAppointmentDTO request)
        {
            await _appointmentService.CreateAppointment(request);
            return StatusCode(StatusCodes.Status201Created, "Create Appointment successfully");
        }

        [HttpPatch(ApiEndpointConstant.Appointment.AppointmentEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update appointment information of account (Inclue basic information update, or cancel appointment)")]
        public async Task<IActionResult> UpdateAppointment([FromRoute]string id, [FromBody]UpdateAppointmentDTO request)
        {
            await _appointmentService.UpdateAppointment(id, request);
            return Ok("Update Appointment successfully");
        }
    }
}
