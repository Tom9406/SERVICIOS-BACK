using Dapper;
using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Application.DTOs;
using Logistica.API.Common;
using System.Data;

namespace Logistica.API.Infrastructure
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ServiceRequestRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private IDbConnection CreateConnection()
        {
            return _connectionFactory.CreateConnection();
        }

        // 🔹 CREATE
        public async Task<long> CreateAsync(int companyId, int userId, int serviceId, string imageUrl, string ip)
        {
            using var db = CreateConnection();

            return await db.QueryFirstAsync<long>(
                "CreateServiceRequest",
                new
                {
                    CompanyID = companyId,
                    UserID = userId,
                    ServiceID = serviceId,
                    ImageUrl = imageUrl,
                    IPAddress = ip
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 CLIENTE
        public async Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(int companyId, int userId)
        {
            using var db = CreateConnection();

            return await db.QueryAsync<ServiceRequestDto>(
                "GetUserServiceRequests",
                new { CompanyID = companyId, UserID = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔥 ADMIN / GESTOR (PAGINADO + SEARCH)
        public async Task<PagedResult<ServiceRequestDto>> GetAdminAsync(
            int companyId,
            int userId,
            int pageNumber,
            int pageSize,
            string? search,
            string? status)
        {
            using var db = CreateConnection();

            using var multi = await db.QueryMultipleAsync(
                "GetServiceRequestsAdmin",
                new
                {
                    CompanyID = companyId,
                    UserID = userId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Search = search,
                    Status = status
                },
                commandType: CommandType.StoredProcedure
            );

            var data = (await multi.ReadAsync<ServiceRequestDto>()).ToList();
            var totalRows = await multi.ReadFirstAsync<int>();

            return new PagedResult<ServiceRequestDto>
            {
                Data = data,
                TotalRows = totalRows
            };
        }

        // 🔹 UPDATE STATUS
        public async Task UpdateStatusAsync(long requestId, int userId, string newStatus, string ip)
        {
            using var db = CreateConnection();

            await db.ExecuteAsync(
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

        // 🔹 VALIDAR PAGO
        public async Task ValidatePaymentAsync(long requestId, int userId, bool approve, string ip)
        {
            using var db = CreateConnection();

            await db.ExecuteAsync(
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

        public async Task<int> CreateAttachmentAsync(
    int companyId,
    int userId,
    long serviceRequestId,
    string nombreArchivo,
    string rutaArchivo,
    int tamañoBytes,
    string ip)
        {
            using var db = CreateConnection();

            return await db.QueryFirstAsync<int>(
                "CreateServiceRequestAttachment",
                new
                {
                    CompanyID = companyId,
                    UserID = userId,
                    ServiceRequestID = serviceRequestId,
                    NombreArchivo = nombreArchivo,
                    RutaArchivo = rutaArchivo,
                    TamañoBytes = tamañoBytes,
                    IPAddress = ip
                },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<ServiceRequestAttachmentDto?> GetAttachmentAsync(
    int companyId,
    long requestId)
        {
            using var db = CreateConnection();

            return await db.QueryFirstOrDefaultAsync<ServiceRequestAttachmentDto>(
                "GetServiceRequestAttachment",
                new
                {
                    CompanyID = companyId,
                    ServiceRequestID = requestId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ServiceRequestDto?> GetByIdAsync(long requestId, int companyId)
        {
            using var db = _connectionFactory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<ServiceRequestDto>(
                "GetServiceRequestById",
                new
                {
                    RequestID = requestId,
                    CompanyID = companyId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}