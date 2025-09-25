using HeartSpace.Api.Models;
using HeartSpace.Application.Services.AppointmentService;
using HeartSpace.Application.Services.AppointmentService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        public async Task<ActionResult<ApiResponse<AppointmentResponse>>> BookAppointment([FromBody] AppointmentBookingRequest request)
        {
            var response = await _appointmentService.CreateAppointmentAsync(request);
            return Ok(response);
        }

    }
}
