using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace ProjectSTSlore
{
    public class SampleContextFactory : IDesignTimeDbContextFactory<HumanResourcesDBContext>
    {
        public HumanResourcesDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HumanResourcesDBContext>();

            /*ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");*/
            optionsBuilder.UseSqlite($"Data Source={MainProgram.homeDirectory}\\HumanResourcesDB.db;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new HumanResourcesDBContext(optionsBuilder.Options);
        }
    }
}
