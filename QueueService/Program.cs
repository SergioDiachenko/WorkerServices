using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.QueueService
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
            monitorLoop.StartMonitorLoop();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MonitorLoop>();
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue>(_ =>
                    {
                        if (!int.TryParse(hostContext.Configuration["QueueCapacity"], out var queueCapacity))
                        {
                            queueCapacity = 100;
                        }

                        return new DefaultBackgroundTaskQueue(queueCapacity);
                    });
                });
    }
}
