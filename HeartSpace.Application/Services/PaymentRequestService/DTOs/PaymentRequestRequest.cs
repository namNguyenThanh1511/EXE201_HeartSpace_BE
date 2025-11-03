using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartSpace.Application.Services.PaymentRequestService.DTOs
{
    public class PaymentRequestRequest
    {
        public Guid AppointmentId { get; set; }
        public decimal RequestAmount { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
    }
}
