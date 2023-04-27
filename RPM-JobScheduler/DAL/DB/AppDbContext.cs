using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RPM_JobScheduler.DAL.Models;

namespace RPM_JobScheduler.DAL.DB;

public class AppDbContext : DbContext
{
    public AppDbContext(string connectionString) : base(GetOptions(connectionString))
    {
    }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
    }

    public DbSet<FuelPriceRecord> FuelPriceRecords { get; set; }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        return new AppDbContext("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true");
    }
}
