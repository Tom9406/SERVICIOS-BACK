namespace Logistica.API.Application.DTOs
{
    public class ServiceRequestAttachmentDto
    {
        public int Id { get; set; }
        public long ServiceRequestID { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public int TamañoBytes { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}
