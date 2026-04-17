using Logistica.API.Application.DTOs;
using Logistica.API.Common;

namespace Logistica.API.Infrastructure.Repositories
{
    public interface IAuditRepository
    {
        Task<PagedResult<AuditLogDto>> GetAuditLogsAsync( int companyId, GetAuditLogsRequestDto request);
    }
}
