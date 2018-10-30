using System.Threading.Tasks;

namespace Azure.Monitor.Abstractions
{
    public interface IMonitorOutput
    {
        Task OutputAsync(MonitorRecord record);
    }
}