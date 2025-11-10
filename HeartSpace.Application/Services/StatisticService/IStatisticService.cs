using HeartSpace.Application.Services.StatisticService.DTOs;

namespace HeartSpace.Application.Services.StatisticService
{
    public interface IStatisticService
    {
        Task<Dictionary<string, decimal>> GetRevenueStatisticsAsync();
        Task<List<DailyRevenueDto>> GetDailyRevenueStatisticsAsync(DateTimeOffset startDate, DateTimeOffset endDate);

    }
}
