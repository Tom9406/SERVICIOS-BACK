using Encomiendas.API.Application.DTOs;
using Logistica.API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Encomiendas.API.Infrastructure.Repositories
{
    public interface IShipmentRepository
    {
        Task<CreateShipmentResponse> CreateShipmentAsync(CreateShipmentRequest request, int userId,  int companyId);
        Task ChangeShipmentStatusAsync(ChangeShipmentStatusRequest request, int userId, int companyId);
        Task<IEnumerable<ShipmentHistoryDto>> GetShipmentHistoryAsync(int shipmentId, int companyId, string userRole);
        Task<ShipmentTrackingResponseDto> GetTrackingAsync(string trackingNumber);
    }
}