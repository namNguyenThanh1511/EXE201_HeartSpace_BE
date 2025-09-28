using HeartSpace.Api.Models;
using HeartSpace.Application.Services.ScheduleService;
using HeartSpace.Application.Services.ScheduleService.DTOs;
using HeartSpace.Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _scheduleService;
        private readonly ICurrentUserService _currentUserService;
        public ScheduleController(IScheduleService scheduleService, ICurrentUserService currentUserService)
        {
            _scheduleService = scheduleService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<ApiResponse<ScheduleResponse>>> CreateSchedule(ScheduleCreation request)
        {
            var result = await _scheduleService.CreateScheduleAsync(request);
            return Created(result);
        }

        [HttpGet("consultant/{consultantId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleResponse>>>> GetSchedulesByConsultantId(Guid consultantId)
        {

            var result = await _scheduleService.GetSchedulesByConsultantIdAsync(consultantId);
            return Ok(result, "Get schedules successfully");
        }

    }
}
