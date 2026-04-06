namespace Logistica.API.Application.DTOs
{
    public class ServiceRequestDto
    {
        public long RequestID { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public string EstimatedTimeText { get; set; }
        public string Category { get; set; }
    }
}
