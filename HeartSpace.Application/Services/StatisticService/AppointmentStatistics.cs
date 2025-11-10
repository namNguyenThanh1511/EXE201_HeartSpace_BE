using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Services.StatisticService
{
    public static class AppointmentStatistics
    {
        public static Dictionary<string, decimal> GetRevenueStatistics(IEnumerable<Appointment> appointments)
        {
            var paidAppointments = appointments
                .Where(a => a.PaymentStatus == PaymentStatus.Paid && !a.IsDeleted);

            var revenueByDay = paidAppointments
                .GroupBy(a => a.CreatedAt.Date)
                .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Sum(a => a.Amount));

            var revenueByMonth = paidAppointments
                .GroupBy(a => new { a.CreatedAt.Year, a.CreatedAt.Month })
                .ToDictionary(
                    g => $"{g.Key.Year}-{g.Key.Month:D2}",
                    g => g.Sum(a => a.Amount)
                );

            var revenueByYear = paidAppointments
                .GroupBy(a => a.CreatedAt.Year)
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Sum(a => a.Amount)
                );

            // Gom tất cả lại 1 object dictionary thống kê tổng quát
            return new Dictionary<string, decimal>
            {
                { "TotalRevenue", paidAppointments.Sum(a => a.Amount) },
                { "TodayRevenue", paidAppointments.Where(a => a.CreatedAt.Date == DateTimeOffset.UtcNow.Date).Sum(a => a.Amount) },
                { "ThisMonthRevenue", paidAppointments.Where(a => a.CreatedAt.Year == DateTimeOffset.UtcNow.Year && a.CreatedAt.Month == DateTimeOffset.UtcNow.Month).Sum(a => a.Amount) },
                { "ThisYearRevenue", paidAppointments.Where(a => a.CreatedAt.Year == DateTimeOffset.UtcNow.Year).Sum(a => a.Amount) },
            };
        }

        public static object GetDetailedRevenueStats(IEnumerable<Appointment> appointments)
        {
            var paidAppointments = appointments
                .Where(a => a.PaymentStatus == PaymentStatus.Paid && !a.IsDeleted);

            return new
            {
                TotalRevenue = paidAppointments.Sum(a => a.Amount),
                RevenueByDay = paidAppointments
                    .GroupBy(a => a.CreatedAt.Date)
                    .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Sum(a => a.Amount)),
                RevenueByMonth = paidAppointments
                    .GroupBy(a => new { a.CreatedAt.Year, a.CreatedAt.Month })
                    .ToDictionary(
                        g => $"{g.Key.Year}-{g.Key.Month:D2}",
                        g => g.Sum(a => a.Amount)
                    ),
                RevenueByYear = paidAppointments
                    .GroupBy(a => a.CreatedAt.Year)
                    .ToDictionary(
                        g => g.Key.ToString(),
                        g => g.Sum(a => a.Amount)
                    )
            };
        }
    }
}
