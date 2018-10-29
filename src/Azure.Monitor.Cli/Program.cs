using System;
using System.Threading.Tasks;
using Azure.Monitor.Outputs;
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

            string connectionString = configuration.GetConnectionString("ServiceBus");

            using (var monitor = new AzureMonitor())
            {
                await monitor.ForServiceBus(connectionString, sb => sb.AllTopics())
                             .OutputConsole()
                             .OutputJsonFile("output.json")
                             .OutputInMemory(out InMemoryOutput o)
                             .StartAsync();

            }
        }
    }
}
