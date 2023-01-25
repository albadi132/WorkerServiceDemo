using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Options;
using WorkerServiceDimo;
using WorkerServiceDimo.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(options =>
    {
        if (OperatingSystem.IsWindows())
        {
            options.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information);
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<ReportsPath>(configuration.GetSection(nameof(ReportsPath)));

        services.AddHostedService<Worker>();

        if (OperatingSystem.IsWindows())
        {
            services.Configure<EventLogSettings>(config =>
            {
                if(OperatingSystem.IsWindows())
                {
                    config.LogName = "Windows Service Demo";
                    config.SourceName = "Windows Service Source";
                }
            });
        }
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();
