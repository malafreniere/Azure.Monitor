using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;

namespace Azure.Monitor
{
    public class CompositeMonitor : IResourceMonitor, IDisposable
    {
        private Queue<IResourceMonitor> _monitors = new Queue<IResourceMonitor>();

        public void AddMonitor(IResourceMonitor monitor)
        {
            _monitors.Enqueue(monitor);
        }

        public void Dispose()
        {
            foreach (var monitor in _monitors.OfType<IDisposable>())
            {
                monitor.Dispose();
            }

            _monitors.Clear();
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