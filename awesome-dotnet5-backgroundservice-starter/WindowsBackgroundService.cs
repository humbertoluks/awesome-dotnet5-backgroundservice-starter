using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace awesome_dotnet5_backgroundservice_starter
{
    public class WindowsBackgroundService : BackgroundService
    {
        private readonly ToDoService _todoService;
        private readonly ILogger<WindowsBackgroundService> _logger;

        public WindowsBackgroundService(
            ToDoService todoService,
            ILogger<WindowsBackgroundService> logger)
        {
            _todoService = todoService;
            _logger = logger;
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

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
    }
}
