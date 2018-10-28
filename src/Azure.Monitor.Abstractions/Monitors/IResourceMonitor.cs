using System;
using System.Threading.Tasks;

namespace Azure.Monitor.Abstractions
{
    public interface IResourceMonitor
    {
        Task MonitorAsync(IMonitorOutput output);
    }
}
