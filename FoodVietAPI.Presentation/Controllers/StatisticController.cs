using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.DTOs.StatisticDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class StatisticController : BaseController<StatisticController>
    {
        private readonly IStatisticService _statisticService;
        public StatisticController(ILogger<StatisticController> logger, IStatisticService statisticService) : base(logger)
        {
            _statisticService = statisticService;
        }

        [HttpGet(ApiEndpointConstant.Statistic.GardenerGeneralStatisticEndpoint)]
        [ProducesResponseType(typeof(GardenerStatisticDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get the general dashboard information of a gardener record by month")]
        public async Task<IActionResult> GetGardenerGeneralDashboardInfo([FromRoute] string id)
        {
            var res = await _statisticService.GetGardenerDashboard(id);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Statistic.GardenerYearOrderStatisticEndpoint)]
        [ProducesResponseType(typeof(List<MonthOrderStatistic>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get the general dashboard information of a gardener record by month")]
        public async Task<IActionResult> GetGardenerYearlyOrderAmount([FromRoute] string id)
        {
            var res = await _statisticService.GetGardenerYearlyOrderAmount(id);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Statistic.GardenerUpcommingAppointmentEndpoint)]
        [ProducesResponseType(typeof(List<ScheduleAppointmentDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get the general dashboard information of a gardener record by month")]
        public async Task<IActionResult> GetInMonthUpcommingAppointment([FromRoute] string id)
        {
            var res = await _statisticService.GetInMonthUpcommingAppointment(id);

            return Ok(res);
        }
    }
}
