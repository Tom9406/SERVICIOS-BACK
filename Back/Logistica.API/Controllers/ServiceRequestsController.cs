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

        private int GetCompanyId() =>
    int.Parse(User.FindFirst("companyId")?.Value ?? throw new Exception("companyId missing"));

        private int GetUserId() =>
            int.Parse(User.FindFirst("userId")?.Value ?? throw new Exception("userId missing"));

        private string GetIP() =>
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";

        // 🔹 CREATE (con comprobante)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
                return BadRequest("Comprobante requerido.");

            var id = await _repo.CreateAsync(
                GetCompanyId(),
                GetUserId(),
                dto.ServiceID,
                dto.ImageUrl,
                GetIP()
            );

            return Ok(new { requestId = id });
        }

        // 🔹 CLIENTE
        [HttpGet("my")]
        public async Task<IActionResult> MyRequests()
        {
            var data = await _repo.GetByUserAsync(GetCompanyId(), GetUserId());
            return Ok(data);
        }

        // 🔹 ADMIN / GESTOR
        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] string? status = null)
        {
            var result = await _repo.GetAdminAsync(
                GetCompanyId(),
                GetUserId(),
                pageNumber,
                pageSize,
                search,
                status
            );

            return Ok(result);
        }

        // 🔹 UPDATE STATUS (operación)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateServiceRequestStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewStatus))
                return BadRequest("Estado requerido.");

            await _repo.UpdateStatusAsync(id, GetUserId(), dto.NewStatus, GetIP());
            return Ok();
        }

        [HttpPut("{id}/payment")]
        public async Task<IActionResult> ValidatePayment(long id, [FromBody] ValidatePaymentDto dto)
        {
            await _repo.ValidatePaymentAsync(id, GetUserId(), dto.Approve, GetIP());
            return Ok(new
            {
                message = dto.Approve ? "Pago validado" : "Pago rechazado"
            });
        }


        [HttpPost("{id}/attachment")]
        public async Task<IActionResult> UploadAttachment(long id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo requerido.");

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("Máximo 10MB.");

            // 🔹 Generar nombre seguro
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folderPath = Path.Combine("wwwroot", "uploads", "service-requests");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, fileName);

            // 🔹 Guardar archivo físico
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🔹 Ruta relativa (lo que guardas en DB)
            var relativePath = $"/uploads/service-requests/{fileName}";

            // 🔹 Guardar en DB
            var adjuntoId = await _repo.CreateAttachmentAsync(
                GetCompanyId(),
                GetUserId(),
                id,
                file.FileName,
                relativePath,
                (int)file.Length,
                GetIP()
            );

            return Ok(new { attachmentId = adjuntoId });
        }

        [HttpGet("{id}/attachment")]
        public async Task<IActionResult> GetAttachment(long id)
        {
            var adjunto = await _repo.GetAttachmentAsync(
                GetCompanyId(),
                id
            );

            if (adjunto == null)
                return NotFound();

            return Ok(adjunto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _repo.GetByIdAsync(id, GetCompanyId());
            return Ok(data);
        }
    }
}