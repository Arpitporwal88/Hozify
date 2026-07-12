using Hozify.Application.Features.Auth.DTOs;
using Hozify.Application.Features.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hozify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp(
        [FromBody] SendOtpRequestDto request)
    {
        var result = await _authService.SendOtpAsync(request);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
        [FromBody] VerifyOtpRequestDto request)
    {
        var result = await _authService.VerifyOtpAsync(request);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutRequestDto request)
    {
        var result = await _authService.LogoutAsync(request);

        return StatusCode(result.StatusCode, result);
    }
}