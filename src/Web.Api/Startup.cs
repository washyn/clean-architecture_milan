using System.Reflection;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Web.Api.Extensions;

namespace Web.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGenWithAuth();

        services
            .AddApplication()
            .AddPresentation()
            .AddInfrastructure(_configuration);

        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddApplication<WebApiModule>();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // TODO: add mapp endpoint
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            // app.ApplyMigrations();
        }

        app.UseHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseRequestContextLogging();

        app.UseSerilogRequestLogging();

        app.UseExceptionHandler();

        app.UseAuthentication();

        app.UseAuthorization();

        // REMARK: If you want to use Controllers, you'll need this.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}
