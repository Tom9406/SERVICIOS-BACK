namespace Encomiendas.API.Application.DTOs
{
    public class CreateShipmentResponse
    {
        public int ShipmentID { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
    }
}