using HeartSpace.Api.Models;
using HeartSpace.Application.Services.ScheduleService;
using HeartSpace.Application.Services.ScheduleService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<ApiResponse<ScheduleResponse>>> CreateSchedule(ScheduleCreation request)
        {
            var result = await _scheduleService.CreateScheduleAsync(request);
            return Created(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleResponse>>>> GetSchedulesByConsultantId()
        {
            var result = await _scheduleService.GetSchedulesByConsultantIdAsync();
            return Ok(result, "Get schedules successfully");
        }

    }
}
