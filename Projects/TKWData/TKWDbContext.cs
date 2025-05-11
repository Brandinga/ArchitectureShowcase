using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TKWData.Models;

namespace TKWData;

public class TKWDbContext : DbContext
{
    public TKWDbContext(DbContextOptions<TKWDbContext> options)
        : base(options)
    {
    }

    // // public TKWDbContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
    // // 		: base()
    // //     {
    // //     }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<Player> Player { get; set; } = null!;
    public DbSet<Session> Session { get; set; } = null!;
}

