
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Logistica.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("userId claim missing");

            return int.Parse(userId);
        }

        protected int GetCompanyId()
        {
            var companyId = User.FindFirst("companyId")?.Value;

            if (string.IsNullOrEmpty(companyId))
                throw new UnauthorizedAccessException("companyId claim missing");

            return int.Parse(companyId);
        }


        protected string GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}