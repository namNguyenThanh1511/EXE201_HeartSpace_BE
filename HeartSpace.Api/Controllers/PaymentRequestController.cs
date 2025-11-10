using HeartSpace.Api.Models;
using HeartSpace.Application.Services.PaymentRequestService;
using HeartSpace.Application.Services.PaymentRequestService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/payment-requests")]
    [ApiController]
    public class PaymentRequestController : BaseController
    {
        // Controller xử lý PaymentRequest (Consultant rút tiền)
        private readonly IPaymentRequestService _paymentRequestService;
        public PaymentRequestController(IPaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PaymentRequestResponse>>>> GetPaymentRequests()
        {
            var paymentRequests = await _paymentRequestService.GetPaymentRequests();
            return Ok(paymentRequests);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PaymentRequestResponse>>> GetPaymentRequestById(Guid id)
        {
            var paymentRequest = await _paymentRequestService.GetPaymentRequestById(id);
            return Ok(paymentRequest);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreatePaymentRequest([FromBody] PaymentRequestRequest paymentRequest)
        {
            var createdRequest = await _paymentRequestService.CreatePaymentRequest(paymentRequest);
            return CreatedAtAction(nameof(GetPaymentRequestById), new { id = createdRequest.Id }, createdRequest);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdatePaymentRequest(Guid id, [FromBody] PaymentRequestRequest paymentRequest)
        {
            paymentRequest.AppointmentId = id;
            var updatedRequest = await _paymentRequestService.UpdatePaymentRequest(paymentRequest);
            return Ok("Update succeed");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentRequest(Guid id)
        {
            var result = await _paymentRequestService.DeletePaymentRequest(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
        [HttpGet("by-appointment/{appointmentId}")]
        public async Task<ActionResult<ApiResponse<PaymentRequestResponse>>> GetPaymentRequestsByAppointmentId(Guid appointmentId)
        {
            var paymentRequests = await _paymentRequestService.GetPaymentRequestByAppointmentId(appointmentId);
            return Ok(paymentRequests);
        }
        [HttpGet("by-consultant/{consultantId}")]
        public async Task<ActionResult<ApiResponse<PaymentRequestResponse>>> GetPaymentRequestsByConsultantId(Guid consultantId)
        {
            var paymentRequests = await _paymentRequestService.GetPaymentRequestByConsultantId(consultantId);
            return Ok(paymentRequests);
        }

    }
}
