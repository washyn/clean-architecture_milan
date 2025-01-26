using Volo.Abp.DependencyInjection;

namespace DbMigrator
{
    public interface IDataSeeder : ITransientDependency
    {
        Task SeedAsync();
    }

    public class DataSeeder : IDataSeeder
    {
        private readonly IEnumerable<IDataSeedContributor> dataSeedContributors;

        public DataSeeder(IEnumerable<IDataSeedContributor> dataSeedContributors)
        {
            this.dataSeedContributors = dataSeedContributors;
        }
        public async Task SeedAsync()
        {
            foreach (var item in dataSeedContributors)
            {
                await item.SeedAsync();
            }
        }
    }
}
