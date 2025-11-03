namespace MpexWebApi.Core.ViewModels.Admin
{
    public class AdminTransactionDto
    {
        public string Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
