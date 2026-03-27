namespace Logistica.API.Application.DTOs.Auth
{
    public class LoginUserDto
    {
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
