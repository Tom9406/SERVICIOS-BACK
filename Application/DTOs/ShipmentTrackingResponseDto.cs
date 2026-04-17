namespace Logistica.API.Application.DTOs
{
    public class ShipmentTrackingResponseDto
    {
        public string TrackingNumber { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string OriginBranch { get; set; }
        public string DestinationBranch { get; set; }
        public DateTime LastUpdate { get; set; }
        public IEnumerable<ShipmentHistoryDto> History { get; set; }
    }
}

