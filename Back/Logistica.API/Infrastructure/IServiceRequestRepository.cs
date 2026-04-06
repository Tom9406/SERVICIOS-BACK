using Logistica.API.Application.DTOs;

namespace Logistica.API.Infrastructure
{
    public interface IServiceRequestRepository
    {
        // 🔹 Crear solicitud + comprobante
        Task<long> CreateAsync(
            int companyId,
            int userId,
            int serviceId,
            string imageUrl, // 🔥 NUEVO
            string ip
        );

        // 🔹 Cliente
        Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(
            int companyId,
            int userId
        );

        // 🔹 Admin / Gestor
        Task<IEnumerable<ServiceRequestDto>> GetAdminAsync(
            int companyId,
            int userId,
            string? status
        );

        // 🔹 Cambiar estado (flujo operativo)
        Task UpdateStatusAsync(
            long requestId,
            int userId,
            string newStatus,
            string ip
        );

        // 🔥 NUEVO → Validar / rechazar pago
        Task ValidatePaymentAsync(
            long requestId,
            int userId,
            bool approve,
            string ip
        );
    }
}