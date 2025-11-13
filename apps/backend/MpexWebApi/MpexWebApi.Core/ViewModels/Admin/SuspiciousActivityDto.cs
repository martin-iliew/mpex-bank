namespace MpexWebApi.Core.ViewModels.Admin
{
    public class SuspiciousActivityDto
    {
        public string UserId { get; set; }
        public string Reason { get; set; }
        public DateTime DetectedAt { get; set; }
    }
}
