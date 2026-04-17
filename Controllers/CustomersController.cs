using Logistica.API.Application.DTOs;
using Logistica.API.Controllers;
using Logistica.API.Infrastructure.Repositores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : BaseController
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    private int GetCompanyId()
    {
        var claim = User.FindFirst("companyId") ?? User.FindFirst("CompanyId");
        if (claim == null) throw new Exception("Token sin companyId");
        return int.Parse(claim.Value);
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("userId") ?? User.FindFirst("UserId");
        if (claim == null) throw new Exception("Token sin userId");
        return int.Parse(claim.Value);
    }

    private string GetRole()
    {
        var claim = User.FindFirst(ClaimTypes.Role)
                 ?? User.FindFirst("role")
                 ?? User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        if (claim == null) throw new Exception("Token sin role");

        return claim.Value;
    }

    private string GetIP()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";
    }

    // 🔹 CREATE
    [HttpPost]
    [Authorize(Roles = "ADMIN_GENERAL,GESTOR_SUPREMO")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var id = await _repository.CreateCustomerAsync(
            GetCompanyId(),
            GetUserId(),
            GetRole(),
            request,
            GetIP());

        return Ok(new { customerID = id });
    }

    // 🔹 UPDATE
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN_GENERAL,GESTOR_SUPREMO")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
      
        if (id != request.CustomerID)
            return BadRequest("El ID no coincide");

        await _repository.UpdateCustomerAsync(
            GetCompanyId(),
            GetUserId(),
            GetRole(),
            request,
            GetIP());

        return NoContent();
    }

    // 🔹 TOGGLE
    [HttpPatch("{id}/toggle")]
    [Authorize(Roles = "ADMIN_GENERAL,GESTOR_SUPREMO")]
    public async Task<IActionResult> Toggle(int id)
    {
        await _repository.ToggleCustomerStatusAsync(
            id,
            GetCompanyId(),
            GetUserId(),
            GetRole(),
            GetIP());

        return NoContent();
    }

    // 🔹 GET (GESTOR SUPREMO)
    [HttpGet]
    [Authorize(Roles = "ADMIN_GENERAL,GESTOR_SUPREMO")]
    public async Task<IActionResult> Get(
        int pageNumber = 1,
        int pageSize = 10,
        string search = null,
        bool? onlyActive = null)
    {
        var result = await _repository.GetCustomersAsync(
            GetCompanyId(),
            pageNumber,
            pageSize,
            search,
            onlyActive);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN_GENERAL,GESTOR_SUPREMO")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repository.GetCustomerByIdAsync(id, GetCompanyId());

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}