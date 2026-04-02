namespace Logistica.API.Application.DTOs
{
    public class AuditLogDto
    {
        public long AuditID { get; set; }
        public int UserID { get; set; }
        public string ActionType { get; set; }
        public string EntityName { get; set; }
        public long EntityID { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string IPAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
