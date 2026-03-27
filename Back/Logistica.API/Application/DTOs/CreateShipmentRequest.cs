namespace Encomiendas.API.Application.DTOs
{
    public class CreateShipmentRequest
    {
        public int CompanyId { get; set; }

        public int SenderCustomerId { get; set; }
        public int ReceiverCustomerId { get; set; }

        public int SenderAddressId { get; set; }
        public int ReceiverAddressId { get; set; }

        public int OriginBranchId { get; set; }
        public int DestinationBranchId { get; set; }

        public decimal Weight { get; set; }

        public string? Description { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public string? Observations { get; set; }

        public int UserId { get; set; }
    }
}