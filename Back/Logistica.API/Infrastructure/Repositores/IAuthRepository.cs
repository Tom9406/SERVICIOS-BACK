using Logistica.API.Application.DTOs;
using Logistica.API.Application.DTOs.Auth;
using Logistica.API.Common;

namespace Logistica.API.Infrastructure.Repositores
{
    public interface IAuthRepository
    {
        Task<LoginUserDto?> LoginAsync( string username, string password);

        
    }
    
}
