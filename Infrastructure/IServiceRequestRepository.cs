using Logistica.API.Application.DTOs;
using Logistica.API.Common;

namespace Logistica.API.Infrastructure
{
    public interface IServiceRequestRepository
    {
        // 🔹 Crear solicitud + comprobante
        Task<long> CreateAsync(
            int companyId,
            int userId,
            int serviceId,
            string imageUrl, // comprobante de pago
            string ip
        );

        // 🔹 NUEVO → Subir adjunto del servicio
        Task<int> CreateAttachmentAsync(
            int companyId,
            int userId,
            long serviceRequestId,
            string nombreArchivo,
            string rutaArchivo,
            int tamañoBytes,
            string ip
        );

        // 🔹 Cliente
        Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(
            int companyId,
            int userId
        );

        // 🔹 Admin / Gestor
        Task<PagedResult<ServiceRequestDto>> GetAdminAsync(
            int companyId,
            int userId,
            int pageNumber,
            int pageSize,
            string? search,
            string? status
        );

        // 🔹 Cambiar estado (flujo operativo)
        Task UpdateStatusAsync(
            long requestId,
            int userId,
            string newStatus,
            string ip
        );

        // 🔥 Validar / rechazar pago
        Task ValidatePaymentAsync(
            long requestId,
            int userId,
            bool approve,
            string ip
        );

        Task<ServiceRequestAttachmentDto?> GetAttachmentAsync(
            int companyId,
            long requestId
        );
        Task<ServiceRequestDto?> GetByIdAsync(long requestId, int companyId);
    }
}