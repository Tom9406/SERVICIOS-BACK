using Dapper;
using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Application.DTOs;
using Logistica.API.Application.DTOs.Auth;
using Logistica.API.Common;
using Logistica.API.Infrastructure.Repositores;
using System.Data;


public class AuthRepository : IAuthRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuthRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<LoginUserDto?> LoginAsync(string username, string password)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new
        {
            Username = username,
            Password = password
        };

        var result = await connection.QuerySingleOrDefaultAsync<LoginUserDto>(
            "Auth_Login",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result; // puede ser null (login fallido)
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