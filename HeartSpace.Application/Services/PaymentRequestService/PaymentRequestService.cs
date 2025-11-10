using HeartSpace.Application.Services.PaymentRequestService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;

namespace HeartSpace.Application.Services.PaymentRequestService
{
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PaymentRequestResponse>> GetPaymentRequests()
        {
            var paymentRequests = await _unitOfWork.PaymentRequests.GetAllAsync();
            return paymentRequests.Select(p => new PaymentRequestResponse
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                ConsultantId = p.ConsultantId,
                RequestAmount = p.RequestAmount,
                BankAccount = p.BankAccount,
                BankName = p.BankName,
                CreatedAt = p.CreatedAt,
                ProcessedAt = p.ProcessedAt,
            }).ToList();
        }
        public async Task<PaymentRequestResponse> GetPaymentRequestById(Guid id)
        {
            var paymentRequest = await _unitOfWork.PaymentRequests.GetByIdAsync(id);
            if (paymentRequest == null)
            {
                throw new EntityNotFoundException("Payment request not found");
            }
            return new PaymentRequestResponse
            {
                Id = paymentRequest.Id,
                AppointmentId = paymentRequest.AppointmentId,
                ConsultantId = paymentRequest.ConsultantId,
                RequestAmount = paymentRequest.RequestAmount,
                BankAccount = paymentRequest.BankAccount,
                BankName = paymentRequest.BankName,
                CreatedAt = paymentRequest.CreatedAt,
                ProcessedAt = paymentRequest.ProcessedAt,
            };
        }
        public async Task<PaymentRequestResponse> CreatePaymentRequest(PaymentRequestRequest paymentRequest)
        {
            var newPaymentRequest = new PaymentRequest
            {
                AppointmentId = paymentRequest.AppointmentId,
                RequestAmount = paymentRequest.RequestAmount,
                BankAccount = paymentRequest.BankAccount,
                BankName = paymentRequest.BankName,
            };
            _unitOfWork.PaymentRequests.Add(newPaymentRequest);
            await _unitOfWork.SaveAsync();
            return new PaymentRequestResponse
            {
                Id = newPaymentRequest.Id,
                AppointmentId = newPaymentRequest.AppointmentId,
                RequestAmount = newPaymentRequest.RequestAmount,
                BankAccount = newPaymentRequest.BankAccount,
                BankName = newPaymentRequest.BankName,
            };
        }
        public async Task<PaymentRequestResponse> UpdatePaymentRequest(PaymentRequestRequest paymentRequest)
        {
            var existingPaymentRequest = await _unitOfWork.PaymentRequests.GetByIdAsync(paymentRequest.AppointmentId);
            if (existingPaymentRequest == null)
            {
                throw new EntityNotFoundException("Payment request not found");
            }
            existingPaymentRequest.RequestAmount = paymentRequest.RequestAmount;
            existingPaymentRequest.BankAccount = paymentRequest.BankAccount;
            existingPaymentRequest.BankName = paymentRequest.BankName;
            _unitOfWork.PaymentRequests.Update(existingPaymentRequest);
            await _unitOfWork.SaveAsync();
            return new PaymentRequestResponse
            {
                Id = existingPaymentRequest.Id,
                AppointmentId = existingPaymentRequest.AppointmentId,
                RequestAmount = existingPaymentRequest.RequestAmount,
                BankAccount = existingPaymentRequest.BankAccount,
                BankName = existingPaymentRequest.BankName,
            };
        }
        public async Task<bool> DeletePaymentRequest(Guid id)
        {
            var paymentRequest = await _unitOfWork.PaymentRequests.GetByIdAsync(id);
            if (paymentRequest == null)
            {
                throw new EntityNotFoundException("Payment request not found");
            }
            _unitOfWork.PaymentRequests.Delete(paymentRequest);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public Task<List<PaymentRequestResponse>> GetPaymentRequestByAppointmentId(Guid appointmentId)
        {
            var query = _unitOfWork.PaymentRequests.FindByCondition(p => p.AppointmentId == appointmentId);
            return Task.FromResult(query.Select(p => new PaymentRequestResponse
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                ConsultantId = p.ConsultantId,
                RequestAmount = p.RequestAmount,
                BankAccount = p.BankAccount,
                BankName = p.BankName,
                CreatedAt = p.CreatedAt,
                ProcessedAt = p.ProcessedAt,
            }).ToList());


        }
        public async Task<List<PaymentRequestResponse>> GetPaymentRequestByConsultantId(Guid consultantId)
        {
            var query = _unitOfWork.PaymentRequests.FindByCondition(p => p.ConsultantId == consultantId);
            return await Task.FromResult(query.Select(p => new PaymentRequestResponse
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                ConsultantId = p.ConsultantId,
                RequestAmount = p.RequestAmount,
                BankAccount = p.BankAccount,
                BankName = p.BankName,
                CreatedAt = p.CreatedAt,
                ProcessedAt = p.ProcessedAt,
            }).ToList());
        }

    }
}