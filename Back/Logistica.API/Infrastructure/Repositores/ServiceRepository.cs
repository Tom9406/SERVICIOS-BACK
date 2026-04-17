using Dapper;
using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Application.DTOs;
using Logistica.API.Common;
using System.Data;
using System.Data.Common;
namespace Logistica.API.Infrastructure.Repositores
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ServiceRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateServiceAsync(
            int companyId,
            int userId,
            string userRole,
            CreateServiceRequest request,
            string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleAsync<int>(
                "dbo.CreateService",
                new
                {
                    CompanyID = companyId,
                    UserID = userId,
                    UserRole = userRole,

                    request.Name,
                    request.Description,
                    request.ReferenceCode,
                    request.Price,
                    request.Cost,
                    request.EstimatedTimeText,
                    request.Category,
                    PermiteAdjunto = request.PermiteAdjunto,
                    IPAddress = ipAddress
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateServiceAsync(
            int companyId,
            int userId,
            string userRole,
            UpdateServiceRequest request,
            string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "dbo.UpdateService",
                new
                {
                    request.ServiceID,
                    CompanyID = companyId,
                    UserID = userId,
                    UserRole = userRole,

                    request.Name,
                    request.Description,
                    request.ReferenceCode,
                    request.Price,
                    request.Cost,
                    request.EstimatedTimeText,
                    request.Category,

                    IPAddress = ipAddress
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ToggleServiceStatusAsync(
            int serviceId,
            int companyId,
            int userId,
            string userRole,
            string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "dbo.ToggleServiceStatus",
                new
                {
                    ServiceID = serviceId,
                    CompanyID = companyId,
                    UserID = userId,
                    UserRole = userRole,
                    IPAddress = ipAddress
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PagedResult<ServiceResponse>> GetServicesAsync(
    int companyId,
    string userRole,
    int pageNumber,
    int pageSize,
    string search,
    bool? onlyActive)
        {
            using var connection = _connectionFactory.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "dbo.GetServices",
                new
                {
                    CompanyID = companyId,
                    UserRole = userRole,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Search = search,
                    OnlyActive = onlyActive
                },
                commandType: CommandType.StoredProcedure
            );

            var data = (await multi.ReadAsync<ServiceResponse>()).ToList();
            var total = await multi.ReadFirstOrDefaultAsync<int>();
            return new PagedResult<ServiceResponse>
            {
                Data = data,
                TotalRows = total
            };
        }

        public async Task<ServiceResponse?> GetServiceByIdAsync(int id, int companyId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<ServiceResponse>(
                @"SELECT *
          FROM Services
          WHERE ServiceID = @ServiceID
          AND CompanyID = @CompanyID",
                new { ServiceID = id, CompanyID = companyId }
            );
        }

        public async Task<IEnumerable<ServiceResponse>> GetServicesByCategoryAsync(
    int companyId,
    string category)
        {
            using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<ServiceResponse>(
                "dbo.GetServicesByCategory",
                new
                {
                    CompanyID = companyId,
                    Category = category
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
