using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Fintech.ExternalService.CloudService.Caching;
using Fintech.ExternalService.StorageHelper;
using Fintech.Library.Business.Concrete;
using Fintech.Library.Business.MappingExtentions.AutoMapper;
using Fintech.Library.Core.Utilities.Security.Jwt;
using Fintech.Library.DataAccess.Concrete;
using Fintech.Library.DataAccess.Concrete.Repository;
using Fintech.Library.Utilities.Extensions;
using Serilog;
using System.Data;
using System.Diagnostics;
using Fintech.ExternalService.CloudService.Caching.Redis;

namespace Fintech.Library.Business.DependencyResolvers.Microsoft;
public static class RegisterServices
{

    public static void ConfigureServicesForWeb(this IServiceCollection services, IConfiguration configuration)
    {
        #region CORE

        services.AddSingleton<ITokenHelper, JwtHelper>();
        services.AddScoped<ICacheService, RedisService>();
        services.AddScoped<IGenericRepository, DapperRepositoryBase>();

        #endregion

        #region BUSINESS

        services.AddScoped<IFixerService, FixerManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ICurrencyService, CurrencyManager>();

        #endregion

        #region SERVICES

        services.AddScoped<IStorageHelper, StorageHelper>();
        services.AddScoped<ICacheService, RedisService>();

        #endregion

        #region DAL


        services.AddScoped<IMerchantDal, MerchantDal>();
        services.AddScoped<IUserDal, EfUserDal>();
        services.AddScoped<ICurrencyHistoryDal, CurrencyHistoryDal>();

        #endregion

        ConfigureCoreServices(services, configuration);
    }

    private static void ConfigureCoreServices(IServiceCollection services, IConfiguration configuration)
    {

        #region Configuration builder

        //IConfiguration _ = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json", false, true)
        //    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
        //    .Build();

        #endregion

        #region Serilog configuration

        Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .WriteTo.Console()
            .CreateLogger();

        #endregion


        //builder.Services.AddSingleton<AsyncLock>();

        services.AddTransient<IDbConnection>(
        config => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));


        services.AddAutoMapper(opt => opt.AddProfile<AutoMapperMappingProfile>());

        services.AddMemoryCache();
        services.AddDistributedMemoryCache();


        //services.AddHealthChecks();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<Stopwatch>();

        DependencyInjectionExtensions.ServiceTool.Create(services);
    }
}
