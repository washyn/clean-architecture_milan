using Volo.Abp.DependencyInjection;

namespace DbMigrator
{
    public interface IDataSeedContributor : ITransientDependency
    {
        Task SeedAsync();
    }
}
