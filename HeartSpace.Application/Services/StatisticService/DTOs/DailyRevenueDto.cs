namespace HeartSpace.Application.Services.StatisticService.DTOs
{
    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ConsultantRevenue { get; set; }
        public decimal SystemRevenue { get; set; }
    }

}
