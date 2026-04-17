namespace Logistica.API.Application.DTOs
{
    public class GetAuditLogsRequestDto
    {
        public int? UserID { get; set; }
        public string? ActionType { get; set; }
        public string? EntityName { get; set; }
        public long? EntityID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string? SearchValue { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
