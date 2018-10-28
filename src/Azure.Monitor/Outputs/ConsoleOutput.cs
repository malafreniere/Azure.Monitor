using System;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Abstractions.Formatters;

namespace Azure.Monitor.Outputs
{
    public class ConsoleOutput : IMonitorOutput
    {
        private readonly IOutputFormatter _formatter;

        public ConsoleOutput(IOutputFormatter formatter)
        {
            _formatter = formatter;
        }

        public Task OutputAsync(MonitorRecord record)
        {
            string output = _formatter.Format(record);

            Console.WriteLine(output);

            return Task.CompletedTask;
        }
    }
}