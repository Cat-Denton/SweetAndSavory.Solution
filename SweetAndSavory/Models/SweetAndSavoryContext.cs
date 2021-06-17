using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SweetAndSavory.Models
{
  public class SweetAndSavoryContext : IdentityDbContext<ApplicationUser>
  {
    public virtual DbSet<Flavor> Flavors { get; set; }
    public virtual DbSet<Treat> Treats { get; set; }
    public DbSet<FlavorTreat> FlavorTreats { get; set; }
    public SweetAndSavoryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}