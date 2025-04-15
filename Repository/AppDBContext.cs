namespace measurement_generator.Repository;

using measurement_generator.Models.Erp;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    public DbSet<Erp> Erps { get; set; }

}

