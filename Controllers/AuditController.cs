using Logistica.API.Application.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logistica.API.Infrastructure.Repositories;

[ApiController]
[Route("api/[controller]")]
public class AuditController : ControllerBase
{
    private readonly IAuditRepository _auditRepository;

    public AuditController(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    [Authorize(Roles = "ADMIN_GENERAL")]
    [HttpPost("logs")]
    public async Task<IActionResult> GetAuditLogs([FromBody] GetAuditLogsRequestDto request)
    {
        var companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var result = await _auditRepository.GetAuditLogsAsync(companyId, request);

        return Ok(new
        {
            success = true,
            data = result
        });
    }
}