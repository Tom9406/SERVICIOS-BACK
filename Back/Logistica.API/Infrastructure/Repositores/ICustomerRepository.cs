using Logistica.API.Application.DTOs;
using Logistica.API.Common;

namespace Logistica.API.Infrastructure.Repositores
{
    public interface ICustomerRepository
    {
        Task<int> CreateCustomerAsync(
            int companyId,
            int userId,
            string userRole,
            CreateCustomerRequest request,
            string ipAddress);

        Task UpdateCustomerAsync(
            int companyId,
            int userId,
            string userRole,
            UpdateCustomerRequest request,
            string ipAddress);

        Task ToggleCustomerStatusAsync(
            int customerId,
            int companyId,
            int userId,
            string userRole,
            string ipAddress);

        Task<PagedResult<CustomerResponse>> GetCustomersAsync(
            int companyId,
            int pageNumber,
            int pageSize,
            string search,
            bool? onlyActive);
    }
}
