using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dashboard.Infra.EF.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VSeriesContext>
{
    public VSeriesContext CreateDbContext(string[] args)
    {
        // Build configuration for getting the connection string
        var optionsBuilder = new DbContextOptionsBuilder<VSeriesContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("ModelContext");
        optionsBuilder.UseSqlServer(connectionString);

        return new VSeriesContext(optionsBuilder.Options);
    }
}

