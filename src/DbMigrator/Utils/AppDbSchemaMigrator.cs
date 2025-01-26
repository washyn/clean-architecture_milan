using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace DbMigrator
{
    public class AppDbSchemaMigrator : ITransientDependency
    {
        private readonly IServiceProvider serviceProvider;

        public AppDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await serviceProvider.GetRequiredService<ApplicationDbContext>()
            .Database
            .MigrateAsync();
        }
    }
}
