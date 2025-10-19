using HeartSpace.Api.Models;
using HeartSpace.Application.Services.ConsultantService;
using HeartSpace.Application.Services.ConsultantService.DTOs;
using HeartSpace.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/consultants")]
    public class ConsultantController : BaseController
    {
        private readonly IConsultantService _consultantService;

        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedList<ConsultantResponse>>>> GetConsultants([FromQuery] ConsultantQueryParams queryParameters)
        {
            var response = await _consultantService.GetConsultantsAsync(queryParameters);
            return Ok(response, "Get consultants success", response.MetaData);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ConsultantResponse>>> GetConsultantById(Guid id)
        {
            var consultant = await _consultantService.GetConsultantByIdAsync(id);
            return Ok(consultant, "Get consultant by id success");
        }
    }
}
