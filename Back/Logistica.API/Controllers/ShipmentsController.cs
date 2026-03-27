using Encomiendas.API.Application.DTOs;
using Encomiendas.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Encomiendas.API.Controllers
{


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentRepository _repository;

        public ShipmentsController(IShipmentRepository repository)
        {
            _repository = repository;
        }

       

[Authorize]
[HttpPost]
public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentRequest request)
{
    var userIdClaim = User.FindFirst("userId")?.Value;
    var companyIdClaim = User.FindFirst("companyId")?.Value;

    if (userIdClaim == null || companyIdClaim == null)
        return Unauthorized("Invalid token");

    request.UserId = int.Parse(userIdClaim);
    request.CompanyId = int.Parse(companyIdClaim);

    var result = await _repository.CreateShipmentAsync(request);

    return Ok(result);
}

        [HttpPost("{id}/status")]
        public async Task<IActionResult> ChangeStatus(
    int id,
    [FromBody] ChangeShipmentStatusRequest request)
        {
            request.ShipmentId = id;

            await _repository.ChangeShipmentStatusAsync(request);

            return Ok(new
            {
                message = "Status updated successfully"
            });
        }

        [HttpGet("track/{trackingNumber}")]
        public async Task<IActionResult> Track(string trackingNumber)
        {
            var result = await _repository.GetTrackingAsync(trackingNumber);

            if (result == null)
                return NotFound(new { error = "Tracking not found" });

            return Ok(result);
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(int id)
        {
            int companyId = int.Parse(User.FindFirst("companyId").Value);
            var result = await _repository.GetShipmentHistoryAsync(id, companyId);
            return Ok(result);
        }




    }
}