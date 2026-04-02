using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Application.DTOs;
using Logistica.API.Common;
using System.Data;
using Dapper;


namespace Logistica.API.Infrastructure.Repositores
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CustomerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateCustomerAsync(
            int companyId,
            int userId,
            string userRole,
            CreateCustomerRequest request,
            string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QuerySingleAsync<int>(
                "dbo.CreateCustomer",
                new
                {
                    CompanyID = companyId,
                    UserID = userId,
                    UserRole = userRole,

                    request.FirstName,
                    request.LastName,
                    request.Phone,
                    request.Email,
                    request.DocumentNumber,
                    request.DocumentType,
                    request.CustomerType,
                    request.BusinessName,

                    IPAddress = ipAddress
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task UpdateCustomerAsync(
            int companyId,
            int userId,
            string userRole,
            UpdateCustomerRequest request,
            string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "dbo.UpdateCustomer",
                new
                {
                    request.CustomerID,
                    CompanyID = companyId,
                    UserID = userId,
                    UserRole = userRole,

                    request.FirstName,
                    request.LastName,
                    request.Phone,
                    request.Email,
                    request.CustomerType,
                    request.BusinessName,

                    IPAddress = ipAddress
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ToggleCustomerStatusAsync(
            int customerId,
            int companyId,
            int userId,
            string userRole,
            string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "dbo.ToggleCustomerStatus",
                new
                {
                    CustomerID = customerId,
                    CompanyID = companyId,
                    UserID = userId,
                    UserRole = userRole,
                    IPAddress = ipAddress
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PagedResult<CustomerResponse>> GetCustomersAsync(
            int companyId,
            int pageNumber,
            int pageSize,
            string search,
            bool? onlyActive)
        {
            using var connection = _connectionFactory.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "dbo.GetCustomers",
                new
                {
                    CompanyID = companyId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Search = search,
                    OnlyActive = onlyActive
                },
                commandType: CommandType.StoredProcedure
            );

            var data = (await multi.ReadAsync<CustomerResponse>()).ToList();
            var total = await multi.ReadSingleAsync<int>();

            return new PagedResult<CustomerResponse>
            {
                Data = data.ToList(),
                TotalRows = total
            };
        }
    }
}
