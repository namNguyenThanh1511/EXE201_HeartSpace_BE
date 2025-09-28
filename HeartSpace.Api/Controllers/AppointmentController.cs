using HeartSpace.Api.Models;
using HeartSpace.Application.Services.AppointmentService;
using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Domain.RequestFeatures;
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<PagedList<AppointmentResponse>>>> GetAppointments([FromQuery] AppointmentQueryParams queryParameters)
        {
            var response = await _appointmentService.GetAppointmentsAsync(queryParameters);
            return Ok(response, "Lấy cuộc hẹn thành công");
        }

        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        public async Task<ActionResult<ApiResponse<AppointmentResponse>>> BookAppointment([FromBody] AppointmentBookingRequest request)
        {
            var response = await _appointmentService.CreateAppointmentAsync(request);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Client,Admin")]
        public async Task<ActionResult<ApiResponse>> UpdateAppointment(Guid id, [FromBody] AppointmentUpdateRequest request)
        {
            var result = await _appointmentService.UpdateAppointmentAsync(id, request);
            if (!result)
                return NotFound("Cập nhật cuộc hẹn thất bại");
            return Ok("Cập nhật cuộc hẹn thành công");
        }

    }
}
