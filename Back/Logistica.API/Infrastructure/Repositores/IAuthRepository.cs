using Logistica.API.Application.DTOs;
using Logistica.API.Application.DTOs.Auth;
using Logistica.API.Common;

namespace Logistica.API.Infrastructure.Repositores
{
    public interface IAuthRepository
    {
        Task<LoginUserDto?> LoginAsync(int companyId, string username, string password);

        
    }
    
}
