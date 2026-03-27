using Logistica.API.Application.DTOs.Auth;

namespace Logistica.API.Infrastructure.Repositores
{
    public interface IJwtService
    {
        string GenerateToken(LoginUserDto user);
    }
}
