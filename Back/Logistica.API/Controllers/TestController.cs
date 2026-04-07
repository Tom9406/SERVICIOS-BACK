using Dapper;
using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Infrastructure.Repositores;
using Microsoft.AspNetCore.Mvc;

namespace Encomiendas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IAuthRepository _authRepository;

        public TestController(
            IDbConnectionFactory connectionFactory,
            IAuthRepository authRepository)
        {
            _connectionFactory = connectionFactory;
            _authRepository = authRepository;
        }

        [HttpGet("db")]
        public async Task<IActionResult> TestConnection()
        {
            var user = await _authRepository.LoginAsync( "admin.asu", "123456");

            return Ok(user);
        }
    }
}