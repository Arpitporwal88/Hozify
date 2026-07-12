using AutoMapper;
using Hozify.Application.Common;
using Hozify.Application.Features.Partners.DTOs;
using Hozify.Application.Features.Partners.Interfaces;
using Hozify.Domain.Constants;
using Hozify.Domain.Entities;
using Hozify.Domain.Enums;
using Hozify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hozify.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly HozifyDbContext _context;
    private readonly IMapper _mapper;

    public PartnerService(HozifyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PartnerResponseDto>> CreatePartnerProfileAsync(
        int userId,
        CreatePartnerDto request)
    {
        // Check User Exists
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);

        if (user == null)
        {
            return ResponseFactory.Failure<PartnerResponseDto>(
                ApiMessages.UserNotFound,
                ApiStatusCode.NotFound);
        }

        // Check User Role
        if (user.Role == null || user.Role.Name != RoleConstants.Partner)
        {
            return ResponseFactory.Failure<PartnerResponseDto>(
                ApiMessages.InvalidPartnerRole,
                ApiStatusCode.BadRequest);
        }

        // Check Already Registered
        var existingPartner = await _context.Partners
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

        if (existingPartner != null)
        {
            return ResponseFactory.Failure<PartnerResponseDto>(
                ApiMessages.UserAlreadyRegisteredAsPartner,
                ApiStatusCode.Conflict);
        }

        // DTO -> Entity
        var partner = _mapper.Map<Partner>(request);

        // Logged-In User
        partner.UserId = userId;

        // Save Partner (Id Generate)
        await _context.Partners.AddAsync(partner);
        await _context.SaveChangesAsync();

        // Generate Partner Code
        partner.PartnerCode = $"HZP{partner.Id:D4}";

        await _context.SaveChangesAsync();

        // Reload Partner with User Information
        partner = await _context.Partners
            .Include(x => x.User)
            .FirstAsync(x => x.Id == partner.Id);

        // Entity -> DTO
        var response = _mapper.Map<PartnerResponseDto>(partner);

        return ResponseFactory.Success(
            response,
            ApiMessages.PartnerCreated,
            ApiStatusCode.Created);
    }

    public async Task<ApiResponse<PartnerResponseDto>> GetMyProfileAsync(int userId)
    {
        // Check Partner Profile
        var partner = await _context.Partners
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

        if (partner == null)
        {
            return ResponseFactory.Failure<PartnerResponseDto>(
                ApiMessages.PartnerNotFound,
                ApiStatusCode.NotFound);
        }

        // Entity -> DTO
        var response = _mapper.Map<PartnerResponseDto>(partner);

        return ResponseFactory.Success(
            response,
            ApiMessages.Success,
            ApiStatusCode.OK);
    }

    public async Task<ApiResponse<PartnerResponseDto>> UpdateMyProfileAsync(
        int userId,
        UpdatePartnerDto request)
    {
        // Check Partner Profile
        var partner = await _context.Partners
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

        if (partner == null)
        {
            return ResponseFactory.Failure<PartnerResponseDto>(
                ApiMessages.PartnerNotFound,
                ApiStatusCode.NotFound);
        }

        // Check Linked User
        if (partner.User == null || partner.User.IsDeleted)
        {
            return ResponseFactory.Failure<PartnerResponseDto>(
                ApiMessages.UserNotFound,
                ApiStatusCode.NotFound);
        }

        // DTO -> Entity
        _mapper.Map(request, partner);

        await _context.SaveChangesAsync();

        // Entity -> DTO
        var response = _mapper.Map<PartnerResponseDto>(partner);

        return ResponseFactory.Success(
            response,
            ApiMessages.PartnerUpdated,
            ApiStatusCode.OK);
    }
}