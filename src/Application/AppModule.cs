using FluentValidation;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Application;
[DependsOn(typeof(AbpAutofacModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddValidatorsFromAssembly(typeof(AppModule).Assembly, includeInternalTypes: true);
    }
}
