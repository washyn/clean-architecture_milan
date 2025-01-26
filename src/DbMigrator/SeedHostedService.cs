using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace DbMigrator
{
    public class SeedHostedService : IHostedService
    {
        private readonly AppDbSchemaMigrator appDbSchemaMigrator;
        private readonly IDataSeeder dataSeeder;
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly ILogger<SeedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SeedHostedService(
            AppDbSchemaMigrator appDbSchemaMigrator,
            IDataSeeder dataSeeder,
            IAbpApplicationWithExternalServiceProvider application,
            ILogger<SeedHostedService> logger,
            IServiceProvider serviceProvider)
        {
            this.appDbSchemaMigrator = appDbSchemaMigrator;
            this.dataSeeder = dataSeeder;
            _application = application;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);

            _logger.LogInformation("MyProjectName module is initialized.");

            await appDbSchemaMigrator.MigrateAsync();
            await dataSeeder.SeedAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
