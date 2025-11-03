using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Services.PaymentRequestService.DTOs
{
    public class PaymentRequestResponse
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public decimal RequestAmount { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public PaymentRequestStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }

    }
}
