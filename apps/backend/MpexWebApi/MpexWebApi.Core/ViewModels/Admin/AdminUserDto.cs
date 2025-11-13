namespace MpexWebApi.Core.ViewModels.Admin
{
    public class AdminUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
