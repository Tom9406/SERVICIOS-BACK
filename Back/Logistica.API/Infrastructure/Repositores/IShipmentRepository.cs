using Encomiendas.API.Application.DTOs;
using Logistica.API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Encomiendas.API.Infrastructure.Repositories
{
    public interface IShipmentRepository
    {
        Task<CreateShipmentResponse> CreateShipmentAsync(CreateShipmentRequest request);
        Task ChangeShipmentStatusAsync(ChangeShipmentStatusRequest request);
        Task<IEnumerable<ShipmentHistoryDto>> GetShipmentHistoryAsync(int shipmentId, int companyId);
        Task<ShipmentTrackingResponseDto> GetTrackingAsync(string trackingNumber);
    }
}