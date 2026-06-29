using AutoMapper;
using Hozify.Application.Common;
using Hozify.Application.Features.Services.DTOs;
using Hozify.Application.Features.Services.Interfaces;
using Hozify.Domain.Constants;
using Hozify.Domain.Entities;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hozify.Infrastructure.Services;

public class ServiceService : IServiceService
{
    private readonly HozifyDbContext _context;
    private readonly IMapper _mapper;

    public ServiceService(HozifyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ServiceResponseDto>> CreateServiceAsync(CreateServiceDto request)
    {
        request.Name = request.Name.Trim();

        // Check Category Exists
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);

        if (!categoryExists)
        {
            return ResponseFactory.Failure<ServiceResponseDto>(
                ApiMessages.InvalidCategory,
                ApiStatusCode.BadRequest);
        }

        // Duplicate Check
        var existingService = await _context.Services
            .FirstOrDefaultAsync(s =>
                s.Name == request.Name &&
                s.CategoryId == request.CategoryId &&
                !s.IsDeleted);

        if (existingService != null)
        {
            return ResponseFactory.Failure<ServiceResponseDto>(
                ApiMessages.ServiceAlreadyExists,
                ApiStatusCode.Conflict);
        }

        var service = _mapper.Map<Service>(request);

        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();

        service = await _context.Services
            .Include(s => s.Category)
            .FirstAsync(s => s.Id == service.Id);

        var response = _mapper.Map<ServiceResponseDto>(service);

        return ResponseFactory.Success(
            response,
            ApiMessages.ServiceCreated,
            ApiStatusCode.Created);
    }

    public async Task<ApiResponse<List<ServiceResponseDto>>> GetAllServicesAsync()
    {
        var services = await _context.Services
            .Include(s => s.Category)
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        var response = _mapper.Map<List<ServiceResponseDto>>(services);

        return ResponseFactory.Success(
            response,
            ApiMessages.Success,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<ServiceResponseDto>> GetServiceByIdAsync(int serviceId)
    {
        var service = await _context.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            return ResponseFactory.Failure<ServiceResponseDto>(
                ApiMessages.ServiceNotFound,
                ApiStatusCode.NotFound);
        }

        var response = _mapper.Map<ServiceResponseDto>(service);

        return ResponseFactory.Success(
            response,
            ApiMessages.Success,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<ServiceResponseDto>> UpdateServiceAsync(int serviceId, UpdateServiceDto request)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            return ResponseFactory.Failure<ServiceResponseDto>(
                ApiMessages.ServiceNotFound,
                ApiStatusCode.NotFound);
        }

        request.Name = request.Name.Trim();

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);

        if (!categoryExists)
        {
            return ResponseFactory.Failure<ServiceResponseDto>(
                ApiMessages.InvalidCategory,
                ApiStatusCode.BadRequest);
        }

        var existingService = await _context.Services
            .FirstOrDefaultAsync(s =>
                s.Name == request.Name &&
                s.CategoryId == request.CategoryId &&
                s.Id != serviceId &&
                !s.IsDeleted);

        if (existingService != null)
        {
            return ResponseFactory.Failure<ServiceResponseDto>(
                ApiMessages.ServiceAlreadyExists,
                ApiStatusCode.Conflict);
        }

        _mapper.Map(request, service);

        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        service = await _context.Services
            .Include(s => s.Category)
            .FirstAsync(s => s.Id == service.Id);

        var response = _mapper.Map<ServiceResponseDto>(service);

        return ResponseFactory.Success(
            response,
            ApiMessages.ServiceUpdated,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<bool>> DeleteServiceAsync(int serviceId)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            return ResponseFactory.Failure<bool>(
                ApiMessages.ServiceNotFound,
                ApiStatusCode.NotFound);
        }

        service.IsDeleted = true;
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ResponseFactory.Success(
            true,
            ApiMessages.ServiceDeleted,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<bool>> RestoreServiceAsync(int serviceId)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == serviceId);

        if (service == null)
        {
            return ResponseFactory.Failure<bool>(
                ApiMessages.ServiceNotFound,
                ApiStatusCode.NotFound);
        }

        if (!service.IsDeleted)
        {
            return ResponseFactory.Failure<bool>(
                ApiMessages.ServiceAlreadyActive,
                ApiStatusCode.BadRequest);
        }

        service.IsDeleted = false;
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ResponseFactory.Success(
            true,
            ApiMessages.ServiceRestored,
            ApiStatusCode.OK);
    }
}