using ForgeRealm.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ForgeRealm.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Building> Buildings => Set<Building>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Resource)
            .WithOne(r => r.Player)
            .HasForeignKey<Resource>(r => r.PlayerId);

        modelBuilder.Entity<Player>()
            .HasMany(p => p.Buildings)
            .WithOne(b => b.Player)
            .HasForeignKey(b => b.PlayerId);
    }
}
