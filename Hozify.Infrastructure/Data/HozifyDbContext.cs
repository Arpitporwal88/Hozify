using Hozify.Domain.Common;
using Hozify.Domain.Constants;
using Hozify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hozify.Infrastructure.Data;

public class HozifyDbContext : DbContext
{
    public HozifyDbContext(DbContextOptions<HozifyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<Partner> Partners => Set<Partner>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = AppDateTime.Now;

        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // User Configuration
        // =========================
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.FullName)
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .HasMaxLength(100);

            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(10);

            entity.HasIndex(x => x.PhoneNumber)
                .IsUnique();

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });

        // =========================
        // Category -> Service
        // =========================
        modelBuilder.Entity<Service>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Services)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // User -> Partner
        // =========================
        modelBuilder.Entity<Partner>()
            .HasOne(p => p.User)
            .WithOne(u => u.Partner)
            .HasForeignKey<Partner>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // User -> Customer
        // =========================
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // User -> Refresh Tokens
        // =========================
        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .Property(x => x.Token)
            .HasMaxLength(500);

        modelBuilder.Entity<RefreshToken>()
            .Property(x => x.DeviceId)
            .HasMaxLength(200);

        modelBuilder.Entity<RefreshToken>()
            .Property(x => x.DeviceName)
            .HasMaxLength(200);

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(x => x.Token)
            .IsUnique();

        // =========================
        // Partner Configuration
        // =========================
        modelBuilder.Entity<Partner>(entity =>
        {
            entity.Property(x => x.PartnerCode)
                .HasMaxLength(20);

            entity.Property(x => x.Bio)
                .HasMaxLength(500);

            entity.Property(x => x.ProfileImage)
                .HasMaxLength(500);

            entity.Property(x => x.ReferralCode)
                .HasMaxLength(20);

            entity.HasIndex(x => x.PartnerCode)
                .IsUnique();

            entity.HasIndex(x => x.UserId)
                .IsUnique();
        });

        // =========================
        // Customer Configuration
        // =========================
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(x => x.ProfileImage)
                .HasMaxLength(500);

            entity.Property(x => x.ReferralCode)
                .HasMaxLength(20);

            entity.HasIndex(x => x.UserId)
                .IsUnique();
        });

        // =========================
        // OTP Configuration
        // =========================
        modelBuilder.Entity<OtpVerification>(entity =>
        {
            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(10);

            entity.Property(x => x.Otp)
                .HasMaxLength(6);

            entity.Property(x => x.Status)
                .HasConversion<string>();

            entity.HasIndex(x => x.PhoneNumber);
        });


    }
}