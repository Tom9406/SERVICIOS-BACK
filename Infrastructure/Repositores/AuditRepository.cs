using Dapper;
using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Application.DTOs;
using Logistica.API.Common;
using Logistica.API.Infrastructure.Repositories;
using System.Data;

public class AuditRepository : IAuditRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuditRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PagedResult<AuditLogDto>> GetAuditLogsAsync(
    int companyId,
    GetAuditLogsRequestDto request)
    {
        using var connection = _connectionFactory.CreateConnection(); 

        var parameters = new DynamicParameters();

        parameters.Add("@CompanyID", companyId);
        parameters.Add("@UserID", request.UserID);
        parameters.Add("@ActionType", request.ActionType);
        parameters.Add("@EntityName", request.EntityName);
        parameters.Add("@EntityID", request.EntityID);
        parameters.Add("@DateFrom", request.DateFrom);
        parameters.Add("@DateTo", request.DateTo);
        parameters.Add("@SearchValue", request.SearchValue);
        parameters.Add("@PageNumber", request.PageNumber);
        parameters.Add("@PageSize", request.PageSize);

        using var multi = await connection.QueryMultipleAsync(
            "dbo.GetAuditLogs",
            parameters,
            commandType: CommandType.StoredProcedure);

        var totalRows = await multi.ReadFirstAsync<int>();
        var data = (await multi.ReadAsync<AuditLogDto>()).ToList();

        return new PagedResult<AuditLogDto>
        {
            TotalRows = totalRows,
            Data = data
        };
    }
}