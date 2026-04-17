namespace Logistica.API.Application.DTOs
{
    public class CreateServiceRequestDto
    {
        public int ServiceID { get; set; }

        // 🔥 URL del comprobante de pago (obligatorio en V1)
        public string ImageUrl { get; set; } = null!;
    }
}