namespace MpexWebApi.Core.ViewModels.Admin
{
    public class DailySummaryReportDto
    {
        public int NewUsersToday { get; set; }
        public int TotalTransactionsToday { get; set; }
        public int SuspiciousActivitiesToday { get; set; }
    }
}
