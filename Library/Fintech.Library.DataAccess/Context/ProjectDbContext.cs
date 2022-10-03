using Fintech.Library.Entities.Concrete;
using Fintech.Library.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Context
{

    public class ProjectDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ProjectDbContext():base()
        {
            _configuration = DependencyInjectionExtensions.ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var httpContextAccessor = DependencyInjectionExtensions.ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
         
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CurrencyHistory> CurrencyHistory { get; set; }
    }
}
