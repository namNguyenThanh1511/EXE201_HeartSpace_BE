using HeartSpace.Api.Models;
using HeartSpace.Application.Services.StatisticService;
using HeartSpace.Application.Services.StatisticService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticController : BaseController
    {
        private readonly IStatisticService _statisticService;
        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        [HttpGet("daily-revenue")]
        public async Task<ActionResult<ApiResponse<List<DailyRevenueDto>>>> GetDaily([FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
        {
            if (startDate > endDate)
            {
                var errorResponse = new ApiResponse<DailyRevenueDto>
                {
                    Message = "Ngày bắt đầu không được lớn hơn ngày kết thúc."
                };
                return BadRequest(errorResponse);
            }

            var result = await _statisticService.GetDailyRevenueStatisticsAsync(startDate, endDate);
            return Ok(result);
        }
    }
}
