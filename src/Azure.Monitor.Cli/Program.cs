using System;
using System.Threading.Tasks;
using Azure.Monitor.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Azure.Monitor.Cli
{
    class Program
    {
        static void Main(string[] args) => Task.WaitAll(MainAsync(args));

        static async Task MainAsync(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddUserSecrets<Program>();
            var configuration = builder.Build();

            var options = configuration.GetSection(nameof(ServiceBusMonitorOptions)).Get<ServiceBusMonitorOptions>();

            ServiceBusMonitor monitor = new ServiceBusMonitor(options);
            monitor.AddOutput(new ConsoleOutput());

            await monitor.MonitorTopicsAsync();
        }
    }
}
