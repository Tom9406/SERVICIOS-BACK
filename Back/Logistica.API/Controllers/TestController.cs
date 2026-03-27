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
            var user = await _authRepository.LoginAsync(1, "admin.asu", "HASH_FAKE_123");

            return Ok(user);
        }
    }
}