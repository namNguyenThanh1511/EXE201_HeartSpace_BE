using HeartSpace.Api.Models;
using HeartSpace.Application.Services.ConsultingService;
using HeartSpace.Application.Services.UserService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/consultings")]
    public class ConsultingController : BaseController
    {
        private readonly IConsultingService _consultingService;
        public ConsultingController(IConsultingService consultingService)
        {
            _consultingService = consultingService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ConsultingsResponse>>>> GetConsultings()
        {
            var response = await _consultingService.GetConsultings();
            return Ok(response, "Get consultants success");
        }

    }
}
