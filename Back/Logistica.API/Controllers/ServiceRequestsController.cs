using Logistica.API.Application.DTOs;
using Logistica.API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistica.API.Controllers
{
    [ApiController]
    [Route("api/servicerequests")]
    [Authorize]
    public class ServiceRequestsController : BaseController
    {
        private readonly IServiceRequestRepository _repo;

        public ServiceRequestsController(IServiceRequestRepository repo)
        {
            _repo = repo;
        }

        private int CompanyId => int.Parse(User.FindFirst("companyId")!.Value);
        private int UserId => int.Parse(User.FindFirst("userId")!.Value);
        private string Ip => HttpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";

        // 🔹 CREATE (con comprobante)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
                return BadRequest("Comprobante requerido.");

            var id = await _repo.CreateAsync(
                CompanyId,
                UserId,
                dto.ServiceID,
                dto.ImageUrl, // 🔥 NUEVO
                Ip
            );

            return Ok(new { requestId = id });
        }

        // 🔹 CLIENTE
        [HttpGet("my")]
        public async Task<IActionResult> MyRequests()
        {
            var data = await _repo.GetByUserAsync(CompanyId, UserId);
            return Ok(data);
        }

        // 🔹 ADMIN / GESTOR
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            var data = await _repo.GetAdminAsync(CompanyId, UserId, status);
            return Ok(data);
        }

        // 🔹 UPDATE STATUS (operación)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateServiceRequestStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewStatus))
                return BadRequest("Estado requerido.");

            await _repo.UpdateStatusAsync(id, UserId, dto.NewStatus, Ip);
            return Ok();
        }

        [HttpPut("{id}/payment")]
        public async Task<IActionResult> ValidatePayment(long id, [FromBody] ValidatePaymentDto dto)
        {
            await _repo.ValidatePaymentAsync(id, UserId, dto.Approve, Ip);
            return Ok(new
            {
                message = dto.Approve ? "Pago validado" : "Pago rechazado"
            });
        }
    }
}