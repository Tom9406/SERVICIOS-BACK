namespace Logistica.API.Application.DTOs
{
    public class CreateServiceRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ReferenceCode { get; set; }

        public decimal Price { get; set; }
        public decimal? Cost { get; set; }

        public string EstimatedTimeText { get; set; }
        public string? Category { get; set; }

        public bool PermiteAdjunto { get; set; } 
    }
}
