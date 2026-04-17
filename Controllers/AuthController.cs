using Encomiendas.API.Application.DTOs;
using Encomiendas.API.Infrastructure.Repositories;
using Logistica.API.Application.DTOs.Auth;
using Logistica.API.Infrastructure.Repositores;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtService _jwtService;

    public AuthController(IAuthRepository authRepository, IJwtService jwtService)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authRepository.LoginAsync(
            
            request.Username,
            request.Password
        );

        if (user == null)
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new LoginResponse
        {
            Token = token
        });
    }
}