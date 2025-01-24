using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Application;
[DependsOn(typeof(AbpAutofacModule))]
public class AppModule : AbpModule
{
    
}
