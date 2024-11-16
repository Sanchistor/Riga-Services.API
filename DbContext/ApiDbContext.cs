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
    public virtual DbSet<BusData> BusData { get; set; } = null!;
    public virtual DbSet<TicketsInfo> TicketsInfo { get; set; } = null!;
    public virtual DbSet<UserTickets> UserTickets { get; set; } = null!;
    public virtual DbSet<CreditCard> CreditCard { get; set; } = null!;
    
}