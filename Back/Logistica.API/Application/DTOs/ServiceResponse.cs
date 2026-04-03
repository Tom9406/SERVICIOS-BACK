namespace Logistica.API.Application.DTOs
{
    public class ServiceResponse
    {
        public int ServiceID { get; set; }
        public int CompanyID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ReferenceCode { get; set; }

        public decimal Price { get; set; }
        public decimal? Cost { get; set; }

        public string EstimatedTimeText { get; set; }
        public string Category { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
