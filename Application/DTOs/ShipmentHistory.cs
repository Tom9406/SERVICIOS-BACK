namespace Logistica.API.Application.DTOs
{
    public class ShipmentHistoryDto
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Branch { get; set; }
        public string Notes { get; set; }
    }
}
