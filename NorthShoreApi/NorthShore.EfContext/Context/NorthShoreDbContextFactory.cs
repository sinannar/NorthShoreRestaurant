using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NorthShore.EfContext.Context
{
    public class NorthShoreDbContextFactory : IDesignTimeDbContextFactory<NorthShoreDbContext>
    {
        public NorthShoreDbContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directoryInfo = new DirectoryInfo(assemblyPath);
            var root = directoryInfo.Parent.Parent.Parent.Parent;
            var webapi = Path.Combine(root.FullName, "NorthShore.Application");

            var conf = new ConfigurationBuilder()
                .SetBasePath(webapi)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true).Build();

            var optionsBuilder = new DbContextOptionsBuilder<NorthShoreDbContext>();
            var connectionString = conf["ConnectionStrings:MsSql"];
            optionsBuilder.UseSqlServer(connectionString);
            return new NorthShoreDbContext(optionsBuilder.Options);
        }
    }
}
