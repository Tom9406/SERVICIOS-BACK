using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistica.API.Controllers
{
    [ApiController]
    [Route("api/uploads")]
    [Authorize]
    public class UploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("payment")]
        [RequestSizeLimit(5 * 1024 * 1024)] // 🔒 5MB
        public async Task<IActionResult> UploadPaymentImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo inválido.");

            // 🔒 Tamaño adicional (doble seguridad)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("El archivo supera el límite de 5MB.");

            // 🔒 Tipos permitidos
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Formato no permitido.");

            // 🔒 Extensión
            var ext = Path.GetExtension(file.FileName).ToLower();
            var allowedExt = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExt.Contains(ext))
                return BadRequest("Extensión inválida.");

            // 📁 Ruta
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // 🧠 Nombre seguro
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            // 💾 Guardar
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🌐 URL
            var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new
            {
                imageUrl = url
            });
        }
    }
}