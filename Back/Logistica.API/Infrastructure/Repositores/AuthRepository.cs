using Dapper;
using Logistica.API.Application.DTOs.Auth;
using Logistica.API.Infrastructure.Repositores;
using System.Data;

using Dapper;
using System.Data;
using Encomiendas.API.Infrastructure.Data;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuthRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<LoginUserDto?> LoginAsync(int companyId, string username, string password)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new
        {
            CompanyID = companyId,
            Username = username,
            Password = password
        };

        var result = await connection.QuerySingleOrDefaultAsync<LoginUserDto>(
            "Auth_Login",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result;
    }
}