namespace Encomiendas.API.Application.DTOs
{
    public class ShipmentTrackingResponse
    {
        public int ShipmentID { get; set; }
        public string TrackingNumber { get; set; }
        public decimal? Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        public string OriginBranch { get; set; }
        public string DestinationBranch { get; set; }
    }
}