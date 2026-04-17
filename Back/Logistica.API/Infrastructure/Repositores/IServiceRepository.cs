using Logistica.API.Application.DTOs;
using Logistica.API.Common;

namespace Logistica.API.Infrastructure.Repositores
{
    public interface IServiceRepository
    {
        Task<int> CreateServiceAsync(
            int companyId,
            int userId,
            string userRole,
            CreateServiceRequest request,
            string ipAddress);

        Task UpdateServiceAsync(
            int companyId,
            int userId,
            string userRole,
            UpdateServiceRequest request,
            string ipAddress);

        Task ToggleServiceStatusAsync(
            int serviceId,
            int companyId,
            int userId,
            string userRole,
            string ipAddress);

        Task<PagedResult<ServiceResponse>> GetServicesAsync(
            int companyId,
            string userRole,
            int pageNumber,
            int pageSize,
            string search,
            bool? onlyActive);

        Task<ServiceResponse?> GetServiceByIdAsync(
            int id,
            int companyId);

        Task<IEnumerable<ServiceResponse>> GetServicesByCategoryAsync(
    int companyId,
    string category
);
    }
}