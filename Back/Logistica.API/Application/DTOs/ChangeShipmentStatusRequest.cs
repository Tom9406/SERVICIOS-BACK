namespace Encomiendas.API.Application.DTOs
{
    public class ChangeShipmentStatusRequest
    {
        public int ShipmentId { get; set; }
        public int NewStatusId { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string? Notes { get; set; }
    }
}