using HeartSpace.Domain.Entities;
using Net.payOS.Types;

namespace HeartSpace.Application.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentLink(Guid appointmentId);

        Task<string> CreatePaymentLink(Appointment appointment);

        WebhookData? VerifyPaymentWebhookData(dynamic webhookBody);
    }
}
