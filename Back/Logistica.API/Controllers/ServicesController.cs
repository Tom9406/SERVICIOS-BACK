using Logistica.API.Application.DTOs;
using Logistica.API.Infrastructure.Repositores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logistica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _repository;

        public ServicesController(IServiceRepository repository)
        {
            _repository = repository;
        }

        private int GetCompanyId() =>
            int.Parse(User.FindFirst("companyId").Value);

        private int GetUserId() =>
            int.Parse(User.FindFirst("userId").Value);

        private string GetRole() =>
            User.FindFirst(ClaimTypes.Role).Value;

        private string GetIP() =>
            HttpContext.Connection.RemoteIpAddress?.ToString();

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceRequest request)
        {
            var id = await _repository.CreateServiceAsync(
                GetCompanyId(),
                GetUserId(),
                GetRole(),
                request,
                GetIP());

            return Ok(new { ServiceID = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateServiceRequest request)
        {
            await _repository.UpdateServiceAsync(
                GetCompanyId(),
                GetUserId(),
                GetRole(),
                request,
                GetIP());

            return NoContent();
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id)
        {
            await _repository.ToggleServiceStatusAsync(
                id,
                GetCompanyId(),
                GetUserId(),
                GetRole(),
                GetIP());

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            int pageNumber = 1,
            int pageSize = 10,
            string search = null,
            bool? onlyActive = null)
        {
            var result = await _repository.GetServicesAsync(
                GetCompanyId(),
                GetRole(),
                pageNumber,
                pageSize,
                search,
                onlyActive);

            return Ok(result);
        }
    }
}
