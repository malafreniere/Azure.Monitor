using System.Threading.Tasks;

namespace Azure.Monitor.Abstractions
{
    public interface IBufferedMonitorOutput : IMonitorOutput
    {
        Task FlushAsync();
    }
}