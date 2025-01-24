using Volo.Abp.Modularity;

namespace Web.Api;

[DependsOn(typeof(Application.AppModule))]
public class WebApiModule : AbpModule
{
    // register module to startup application...
}
