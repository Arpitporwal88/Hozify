using Microsoft.EntityFrameworkCore;
using Hozify.Domain.Entities;

namespace Hozify.Infrastructure.Data;

public class HozifyDbContext : DbContext
{
    public HozifyDbContext(DbContextOptions<HozifyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();
}