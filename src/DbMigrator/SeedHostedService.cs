using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace DbMigrator
{
    public class SeedHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly ILogger<SeedHostedService> _logger;
        private readonly IEnumerable<IDataSeedContributor> _dataSeedContributors;
        private readonly IServiceProvider _serviceProvider;

        public SeedHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            ILogger<SeedHostedService> logger,
            IEnumerable<IDataSeedContributor> dataSeedContributors,
            IServiceProvider serviceProvider)
        {
            _application = application;
            _logger = logger;
            _dataSeedContributors = dataSeedContributors;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);

            _logger.LogInformation("MyProjectName module is initialized.");

            foreach (var dataSeedContributor in _dataSeedContributors)
            {
                await dataSeedContributor.SeedAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
