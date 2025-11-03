using HeartSpace.Application.Services.PaymentRequestService.DTOs;

namespace HeartSpace.Application.Services.PaymentRequestService
{
    public interface IPaymentRequestService
    {
        Task<List<PaymentRequestResponse>> GetPaymentRequests();
        Task<PaymentRequestResponse> GetPaymentRequestById(Guid id);
        Task<PaymentRequestResponse> CreatePaymentRequest(PaymentRequestRequest paymentRequest);
        Task<PaymentRequestResponse> UpdatePaymentRequest(PaymentRequestRequest paymentRequest);
        Task<bool> DeletePaymentRequest(Guid id);
        Task<List<PaymentRequestResponse>> GetPaymentRequestByAppointmentId(Guid appointmentId);
        Task<List<PaymentRequestResponse>> GetPaymentRequestByConsultantId(Guid consultantId);
    }
}