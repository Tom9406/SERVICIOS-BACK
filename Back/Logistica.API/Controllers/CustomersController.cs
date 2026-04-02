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

    private int GetCompanyId() =>
        int.Parse(User.FindFirst("companyId").Value);

    private int GetUserId() =>
        int.Parse(User.FindFirst("userId").Value);

    private string GetRole() =>
        User.FindFirst(ClaimTypes.Role).Value;

    private string GetIP() =>
        HttpContext.Connection.RemoteIpAddress?.ToString();

    // 🔹 CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var id = await _repository.CreateCustomerAsync(
            GetCompanyId(),
            GetUserId(),
            GetRole(),
            request,
            GetIP());

        return Ok(new { CustomerID = id });
    }

    // 🔹 UPDATE
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerRequest request)
    {
        await _repository.UpdateCustomerAsync(
            GetCompanyId(),
            GetUserId(),
            GetRole(),
            request,
            GetIP());

        return NoContent();
    }

    // 🔹 TOGGLE STATUS
    [HttpPatch("{id}/toggle")]
    [Authorize(Roles = "ADMIN_GENERAL")]
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

    // 🔹 GET PAGINADO
    [HttpGet]
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
}