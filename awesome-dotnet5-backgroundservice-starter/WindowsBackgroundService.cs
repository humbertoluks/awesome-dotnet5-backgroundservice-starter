using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace awesome_dotnet5_backgroundservice_starter
{
    public class WindowsBackgroundService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ToDoService _todoService;
        private readonly ILogger<WindowsBackgroundService> _logger;

        public WindowsBackgroundService(
            IConfiguration configuration,
            ToDoService todoService,
            ILogger<WindowsBackgroundService> logger)
        {
            _configuration = configuration;
            _todoService = todoService;
            _logger = logger;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    string todo = await _todoService.GetTodoAsync();
                    _logger.LogWarning(todo);

                    await Task.Delay(TimeSpan.FromMinutes(_configuration.GetValue<short>("TaskDelayMinutes")), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "There was a problem executing the service");
                    throw;
                }
            }
        }
    }
}
