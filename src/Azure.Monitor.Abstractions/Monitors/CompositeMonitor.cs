using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.Monitor.Abstractions
{
    public class CompositeMonitor : IResourceMonitor
    {
        private Queue<IResourceMonitor> _monitors = new Queue<IResourceMonitor>();

        public void AddMonitor(IResourceMonitor monitor)
        {
            _monitors.Enqueue(monitor);
        }

        public async Task MonitorAsync(IMonitorOutput output)
        {
            foreach (var monitor in _monitors)
            {
                await monitor.MonitorAsync(output).ConfigureAwait(false);
            }
        }
    }
}