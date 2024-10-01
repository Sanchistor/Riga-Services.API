using Microsoft.EntityFrameworkCore;
using riga.services.Models;

namespace riga.services.DbContext;

public class ApiDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public virtual DbSet<User> Users { get; set; } = null!;

}