using Infrastructure;
using Volo.Abp.Modularity;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api;

[DependsOn(typeof(InfraestructureModule))]
public class WebApiModule : AbpModule
{
    // register module to startup application...

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        services.AddSwaggerGenWithAuth();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // REMARK: If you want to use Controllers, you'll need this.
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}
