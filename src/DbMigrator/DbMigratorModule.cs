using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Modularity;

namespace DbMigrator
{
    [DependsOn(typeof(Infrastructure.InfraestructureModule))]
    public class DbMigratorModule : AbpModule
    {
        override public void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();

            context.Services.AddHostedService<SeedHostedService>();
        }
    }
}
