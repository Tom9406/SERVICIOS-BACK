using Logistica.API.Application.DTOs.Auth;

namespace Logistica.API.Infrastructure.Repositores
{
    public interface IAuthRepository
    {
        Task<LoginUserDto?> LoginAsync(int companyId, string username, string password);
    }
    
}
