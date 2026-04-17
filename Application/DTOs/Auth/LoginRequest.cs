namespace Logistica.API.Application.DTOs.Auth
{
    public class LoginRequest
    {
        public int CompanyId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
