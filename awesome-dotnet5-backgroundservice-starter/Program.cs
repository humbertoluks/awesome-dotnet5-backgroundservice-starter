using awesome_dotnet5_backgroundservice_starter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.File(@"C:\temp\workerservice\LogFile.txt")
        .CreateLogger();
try
{
    Log.Information("Starting up the service");

    using IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService(options =>
        {
            options.ServiceName = ".NET To Do Service";
        })
        .ConfigureServices(services =>
        {
            services.AddHostedService<WindowsBackgroundService>();
            services.AddHttpClient<ToDoService>();
        })
        .UseSerilog()
        .Build();

    await host.RunAsync();

    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service");
    return;
}
finally
{
    Log.CloseAndFlush();
}

