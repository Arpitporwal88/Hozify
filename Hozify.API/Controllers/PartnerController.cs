using System.Security.Claims;
using Hozify.Application.Features.Partners.DTOs;
using Hozify.Application.Features.Partners.Interfaces;
using Hozify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hozify.API.Controllers;

[ApiController]
[Route("api/partners")]
[Authorize(Roles = RoleConstants.Partner)]
public class PartnerController : ControllerBase
{
    private readonly IPartnerService _partnerService;

    public PartnerController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    /// <summary>
    /// Create Partner Profile
    /// </summary>
    [HttpPost("profile")]
    public async Task<IActionResult> CreatePartnerProfileAsync(CreatePartnerDto request)
    {
        var userId = GetLoggedInUserId();

        var result = await _partnerService.CreatePartnerProfileAsync(userId, request);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get Logged-In Partner Profile
    /// </summary>
    [HttpGet("profile")]
    public async Task<IActionResult> GetMyProfileAsync()
    {
        var userId = GetLoggedInUserId();

        var result = await _partnerService.GetMyProfileAsync(userId);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update Logged-In Partner Profile
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateMyProfileAsync(UpdatePartnerDto request)
    {
        var userId = GetLoggedInUserId();

        var result = await _partnerService.UpdateMyProfileAsync(userId, request);

        return StatusCode(result.StatusCode, result);
    }

    #region Private Methods

    private int GetLoggedInUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId) ||
            !int.TryParse(userId, out var id))
        {
            throw new UnauthorizedAccessException(ApiMessages.InvalidToken);
        }

        return id;
    }

    #endregion
}