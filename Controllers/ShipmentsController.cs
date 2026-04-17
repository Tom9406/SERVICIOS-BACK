using Encomiendas.API.Application.DTOs;
using Encomiendas.API.Infrastructure.Repositories;
using Logistica.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : BaseController
{
    private readonly IShipmentRepository _repository;

    public ShipmentsController(IShipmentRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentRequest request)
    {
        var userId = GetUserId();
        var companyId = GetCompanyId();

        var result = await _repository.CreateShipmentAsync(request, userId, companyId);

        return Ok(new
        {
            success = true,
            data = result,
            error = (object?)null
        });
    }

    [HttpPost("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeShipmentStatusRequest request)
    {
        var userId = GetUserId();
        var companyId = GetCompanyId();

        request.ShipmentId = id;

        await _repository.ChangeShipmentStatusAsync(request, userId, companyId);

        return Ok(new
        {
            success = true,
            data = new { message = "Status updated successfully" },
            error = (object?)null
        });
    }


    [Authorize(Roles = "ADMIN_GENERAL,CAJERO")]
    [AllowAnonymous]
    [HttpGet("track/{trackingNumber}")]
    public async Task<IActionResult> Track(string trackingNumber)
    {
        var result = await _repository.GetTrackingAsync(trackingNumber);

        if (result == null)
            throw new Exception("Tracking not found"); // lo manejará el middleware

        return Ok(new
        {
            success = true,
            data = result,
            error = (object?)null
        });
    }


    [Authorize(Roles = "ADMIN_GENERAL")]
    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory(int id)
    {
        var companyId = GetCompanyId();
        var role = GetUserRole();

        var result = await _repository.GetShipmentHistoryAsync(id, companyId, role);

        return Ok(new
        {
            success = true,
            data = result,
            error = (object?)null
        });
    }
}