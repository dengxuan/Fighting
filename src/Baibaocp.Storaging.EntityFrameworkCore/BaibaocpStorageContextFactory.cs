using Fighting.Storaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Baibaocp.Storaging.EntityFrameworkCore
{
    public class BaibaocpStorageContextFactory : IDesignTimeDbContextFactory<BaibaocpStorageContext>
    {
        public BaibaocpStorageContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<BaibaocpStorageContext>();
            //optionsBuilder.UseMySql(builder.GetConnectionString("Fighting.Storage"));
            return new BaibaocpStorageContext(new StorageOptions { DefaultNameOrConnectionString = builder.GetConnectionString("Fighting.Storage") }, optionsBuilder.Options);
        }
    }
}
