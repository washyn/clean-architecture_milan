using System.Text;
using Application;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using Volo.Abp.Modularity;

namespace Infrastructure
{
    [DependsOn(typeof(AppModule))]
    public class InfraestructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = context.Services.GetConfiguration();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();


            string? connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<ApplicationDbContext>(
                options => options
                    .UseNpgsql(connectionString, npgsqlOptions =>
                        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                    .UseSnakeCaseNamingConvention());
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());


            services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                       .AddJwtBearer(o =>
                       {
                           o.RequireHttpsMetadata = false;
                           o.TokenValidationParameters = new TokenValidationParameters
                           {
                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                               ValidIssuer = configuration["Jwt:Issuer"],
                               ValidAudience = configuration["Jwt:Audience"],
                               ClockSkew = TimeSpan.Zero
                           };
                       });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenProvider, TokenProvider>();


            services.AddAuthorization();
            services.AddScoped<PermissionProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        }
    }
}
