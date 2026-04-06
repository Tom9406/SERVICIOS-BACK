using Logistica.API.Application.DTOs;
using System.Data;
using Dapper;

namespace Logistica.API.Infrastructure
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly IDbConnection _db;

        public ServiceRequestRepository(IDbConnection db)
        {
            _db = db;
        }

        // 🔹 CREATE (con comprobante)
        public async Task<long> CreateAsync(int companyId, int userId, int serviceId, string imageUrl, string ip)
        {
            var result = await _db.QueryFirstAsync<long>(
                "CreateServiceRequest",
                new
                {
                    CompanyID = companyId,
                    UserID = userId,
                    ServiceID = serviceId,
                    ImageUrl = imageUrl, // 🔥 NUEVO
                    IPAddress = ip
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        // 🔹 CLIENTE
        public async Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(int companyId, int userId)
        {
            return await _db.QueryAsync<ServiceRequestDto>(
                "GetUserServiceRequests",
                new { CompanyID = companyId, UserID = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 ADMIN / GESTOR
        public async Task<IEnumerable<ServiceRequestDto>> GetAdminAsync(int companyId, int userId, string? status)
        {
            return await _db.QueryAsync<ServiceRequestDto>(
                "GetServiceRequestsAdmin",
                new { CompanyID = companyId, UserID = userId, Status = status },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 CAMBIO DE ESTADO (operación)
        public async Task UpdateStatusAsync(long requestId, int userId, string newStatus, string ip)
        {
            await _db.ExecuteAsync(
                "UpdateServiceRequestStatus",
                new
                {
                    RequestID = requestId,
                    NewStatus = newStatus,
                    UserID = userId,
                    IPAddress = ip
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ValidatePaymentAsync(long requestId, int userId, bool approve, string ip)
        {
            await _db.ExecuteAsync(
                "ValidateServiceRequestPayment",
                new
                {
                    RequestID = requestId,
                    UserID = userId,
                    Approve = approve,
                    IPAddress = ip
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}