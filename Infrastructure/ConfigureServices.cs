using Application.Common.Interfaces.Auth;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Auth;
using Infrastructure.Services.Support;
using Application.Common.Interfaces.Logger;
using Infrastructure.Log;
using Application.Common.Interfaces;
using Infrastructure.Services.DateTime;
using Application.Common.Methodes;
using Application.Common.Interfaces.Services;
using Application.Common.InterFaces.DbContext;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            string envirment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            services.AddDbContext<AdminDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DotekAdmin"),
                builder => builder.MigrationsAssembly(typeof(AdminDbContext).Assembly.FullName));
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            });
            services.AddScoped<IAdminDbContext>(provider => provider.GetRequiredService<AdminDbContext>());



            services.AddScoped<IAdminLogDbContext>(provider => provider.GetRequiredService<AdminLogDbContext>());
            services.AddDbContext<AdminLogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DotekAdminLog"),
                builder => builder.MigrationsAssembly(typeof(AdminLogDbContext).Assembly.FullName)));

           

            var serviceProvider = services.BuildServiceProvider();
            var myService = serviceProvider.GetService<IAdminDbContext>();


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("Redis");
                options.InstanceName = "Dotek";
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfiguration = ConfigurationOptions.Parse(configuration.GetValue<string>("Redis"));

                // Ensure the connection can failover to a new master if needed
                redisConfiguration.AbortOnConnectFail = false;

                // Optional: Enable admin commands if necessary (use with caution)
                redisConfiguration.AllowAdmin = true;

                // Explicitly disable replica-only commands if your application doesn't need them
                redisConfiguration.CommandMap = CommandMap.Default;

                // Optionally, add retry or other policies
                redisConfiguration.ConnectRetry = 3;

                return ConnectionMultiplexer.Connect(redisConfiguration);
            });

            services.AddSingleton<IDistributedCache, RedisCache>(sp =>
            {
                var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return new RedisCache(new RedisCacheOptions
                {
                    Configuration = connectionMultiplexer.Configuration,
                    InstanceName = "Tax",
                    ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer)
                });
            });


            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();
            //services.AddScoped<IRedisStore, RedisStore>();
            services.AddSingleton<IExecutionMode, ExecutionModeSettings>();
            services.AddScoped<IShahkarValidation,ShahkarValidate>();
            services.AddScoped<ISMS, SMS>();
            services.AddScoped<ILogger, Logger>();
            services.AddScoped<IDateTime, DateTimeService>();
    

          


            

            return services;
        }
    }
}