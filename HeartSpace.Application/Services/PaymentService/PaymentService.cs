using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;

namespace HeartSpace.Application.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOsService _payOsService;

        private readonly IConfiguration _config;
        public PaymentService(IUnitOfWork unitOfWork, PayOsService payOsService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _payOsService = payOsService;
            _config = configuration;
        }
        public async Task<string> CreatePaymentLink(Guid appointmentId)
        {
            // Get the order and its items  
            var foundAppointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId) ?? throw new EntityNotFoundException("Order not found");

            // Prepare item list for PayOS  
            var item = new ItemData(
                          name: "Appointment",  // Hoặc bỏ named arguments nếu không cần
                          quantity: 1,
                          price: (int)foundAppointment.Amount
                      );
            var items = new List<ItemData> { item };

            // Convert order.Id to long (cải thiện: dùng Guid.ToString() và parse, hoặc dùng timestamp nếu cần unique)


            var returnUrl = _config.GetSection("PayOS:ReturnUrl").Value;
            var cancelUrl = _config.GetSection("PayOS:CancelUrl").Value;

            // Prepare payment data for PayOS  
            var paymentData = new PaymentData(
                orderCode: foundAppointment.OrderCode,
                amount: (int)foundAppointment.Amount,
                description: $"PAYORDER",
                items: items,
                returnUrl: returnUrl,
                cancelUrl: cancelUrl
            );

            // Gọi public method của PayOsService thay vì access private field
            var paymentResult = await _payOsService.CreatePaymentLinkAsync(paymentData);

            if (paymentResult == null || string.IsNullOrEmpty(paymentResult.checkoutUrl))
            {
                throw new InvalidOperationException("Tạo link thanh toán thất bại từ PayOS.");
            }

            // Optional: Lưu payment info vào DB (ví dụ: order.PaymentLink = paymentResult.checkoutUrl; await _unitOfWork.SaveAsync();)

            return paymentResult.checkoutUrl;  // Trả về link để redirect client
        }

        public async Task<string> CreatePaymentLink(Appointment foundAppointment)
        {
            // Prepare item list for PayOS  
            var item = new ItemData(
                          name: "Appointment",  // Hoặc bỏ named arguments nếu không cần
                          quantity: 1,
                          price: (int)foundAppointment.Amount
                      );
            var items = new List<ItemData> { item };



            var returnUrl = _config.GetSection("PayOS:ReturnUrl").Value;
            var cancelUrl = _config.GetSection("PayOS:CancelUrl").Value;

            // Prepare payment data for PayOS  
            var paymentData = new PaymentData(
                orderCode: foundAppointment.OrderCode,
                amount: (int)foundAppointment.Amount,
                description: $"PAYORDER",
                items: items,
                returnUrl: returnUrl,
                cancelUrl: cancelUrl
            );

            // Gọi public method của PayOsService thay vì access private field
            var paymentResult = await _payOsService.CreatePaymentLinkAsync(paymentData);

            if (paymentResult == null || string.IsNullOrEmpty(paymentResult.checkoutUrl))
            {
                throw new InvalidOperationException("Tạo link thanh toán thất bại từ PayOS.");
            }

            // Optional: Lưu payment info vào DB (ví dụ: order.PaymentLink = paymentResult.checkoutUrl; await _unitOfWork.SaveAsync();)

            return paymentResult.checkoutUrl;  // Trả về link để redirect client
        }

        public WebhookData? VerifyPaymentWebhookData(dynamic webhookBody)
        {
            return _payOsService.VerifyPaymentWebhookData(webhookBody);
        }
    }
}
