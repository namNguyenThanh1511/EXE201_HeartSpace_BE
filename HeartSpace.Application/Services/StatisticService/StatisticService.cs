
using HeartSpace.Application.Services.StatisticService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;

namespace HeartSpace.Application.Services.StatisticService
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Dictionary<string, decimal>> GetRevenueStatisticsAsync()
        {
            // 🔹 Lấy danh sách appointment đã thanh toán thành công
            var paidAppointments = await _unitOfWork.Appointments.GetAllAsync();
            paidAppointments = paidAppointments
                .Where(a => a.Status == AppointmentStatus.Completed && !a.IsDeleted)
                .ToList();

            if (!paidAppointments.Any())
            {
                return new Dictionary<string, decimal>
                    {
                        { "TotalRevenue", 0 },
                        { "TodayRevenue", 0 },
                        { "ThisMonthRevenue", 0 },
                        { "ThisYearRevenue", 0 }
                    };
            }

            var now = DateTimeOffset.UtcNow;

            // 🔹 Tính toán các mốc thời gian
            var today = now.Date;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            // 🔹 Doanh thu theo từng giai đoạn
            decimal totalRevenue = paidAppointments.Sum(a => a.Amount * 0.3m); // system takes 30% cut
            decimal todayRevenue = paidAppointments
                .Where(a => a.CreatedAt.Date == today)
                .Sum(a => a.Amount);

            decimal thisMonthRevenue = paidAppointments
                .Where(a => a.CreatedAt.Year == currentYear && a.CreatedAt.Month == currentMonth)
                .Sum(a => a.Amount);

            decimal thisYearRevenue = paidAppointments
                .Where(a => a.CreatedAt.Year == currentYear)
                .Sum(a => a.Amount);

            // 🔹 Trả về dictionary kết quả
            return new Dictionary<string, decimal>
                {
                    { "TotalRevenue", totalRevenue },
                    { "TodayRevenue", todayRevenue },
                    { "ThisMonthRevenue", thisMonthRevenue },
                    { "ThisYearRevenue", thisYearRevenue }
                };
        }

        public async Task<Dictionary<Guid, decimal>> GetConsultantRevenueAsync()
        {

            throw new NotImplementedException();
        }
        public async Task<List<DailyRevenueDto>> GetDailyRevenueStatisticsAsync(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var completedAppointments = _unitOfWork.Appointments.FindByCondition(a =>
                a.Status == AppointmentStatus.Completed &&
                !a.IsDeleted &&
                a.UpdatedAt.Date >= startDate.Date &&
                a.UpdatedAt.Date <= endDate.Date);


            var grouped = completedAppointments
                .GroupBy(a => a.UpdatedAt.Date) // nhóm theo ngày hoàn thành
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(a => a.EscrowAmount),
                    ConsultantRevenue = g.Sum(a => a.EscrowAmount * 0.7m),
                    SystemRevenue = g.Sum(a => a.EscrowAmount * 0.3m)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return grouped;
        }



    }
}
