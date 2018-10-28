using System;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Outputs;

namespace Azure.Monitor
{
    public class AzureMonitor
    {
        private readonly CompositeOutput _outputs = new CompositeOutput();
        private readonly CompositeMonitor _monitors = new CompositeMonitor();

        public void AddOutput(IMonitorOutput output) => _outputs.AddOutput(output);
        public void AddMonitor(IResourceMonitor monitor) => _monitors.AddMonitor(monitor);

        public async Task StartAsync()
        {
            await _monitors.MonitorAsync(_outputs).ConfigureAwait(false);
        }
    }
}
