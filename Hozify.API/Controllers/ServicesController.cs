using Hozify.Application.Features.Services.DTOs;
using Hozify.Application.Features.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hozify.API.Controllers;

[Route("api/v1/services")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateService(CreateServiceDto request)
    {
        var result = await _serviceService.CreateServiceAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        var result = await _serviceService.GetAllServicesAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById(int id)
    {
        var result = await _serviceService.GetServiceByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, UpdateServiceDto request)
    {
        var result = await _serviceService.UpdateServiceAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var result = await _serviceService.DeleteServiceAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> RestoreService(int id)
    {
        var result = await _serviceService.RestoreServiceAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}