using Application.Abstractions.Data;
using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace DbMigrator
{
    [DependsOn(typeof(Application.AppModule))]
    public class MyProjectNameModule : AbpModule
    {
        override public void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();
            context.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            context.Services.AddHostedService<MyProjectNameHostedService>();
        }
    }
    public class MyProjectNameHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly ILogger<MyProjectNameHostedService> _logger;
        private readonly IEnumerable<IDataSeedContributor> _dataSeedContributors;
        private readonly IServiceProvider _serviceProvider;

        public MyProjectNameHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            ILogger<MyProjectNameHostedService> logger,
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

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutofac()
                // .UseSerilog()
                .ConfigureAppConfiguration((context, config) =>
                {
                    //setup your additional configuration sources
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication<MyProjectNameModule>();
                });
    }
    public interface IDataSeedContributor : ITransientDependency
    {
        Task SeedAsync();
    }
}
